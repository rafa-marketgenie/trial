import { createContext, useContext, useState, useEffect, useCallback, useRef } from 'react';
import * as notesApi from '../api/notes';
import * as noteGroupsApi from '../api/noteGroups';
import { useAuth } from './AuthContext';

const NotesContext = createContext(null);

const POSITIONS_KEY = 'note_positions';

function loadPositions() {
  try {
    const raw = localStorage.getItem(POSITIONS_KEY);
    return raw ? JSON.parse(raw) : {};
  } catch {
    return {};
  }
}

function savePositions(openNotes) {
  const positions = {};
  openNotes.forEach((value, key) => {
    positions[key] = { x: value.x, y: value.y, width: value.width, height: value.height };
  });
  localStorage.setItem(POSITIONS_KEY, JSON.stringify(positions));
}

export function NotesProvider({ children }) {
  const { user, isAuthenticated } = useAuth();
  const [notes, setNotes] = useState([]);
  const [openNotes, setOpenNotes] = useState(new Map());
  const [nextZIndex, setNextZIndex] = useState(1);
  const [groups, setGroups] = useState([]);
  const initialLoadDone = useRef(false);

  // Load notes and groups when authenticated
  useEffect(() => {
    if (isAuthenticated && user?.id && !initialLoadDone.current) {
      initialLoadDone.current = true;
      loadNotes();
      loadGroups();
    }
    if (!isAuthenticated) {
      initialLoadDone.current = false;
      setNotes([]);
      setOpenNotes(new Map());
      setNextZIndex(1);
      setGroups([]);
    }
  }, [isAuthenticated, user]);

  const loadNotes = useCallback(async () => {
    if (!user?.id) return;
    try {
      const data = await notesApi.getByUser(user.id);
      setNotes(data);

      // Restore saved positions for any previously open notes
      const saved = loadPositions();
      if (Object.keys(saved).length > 0) {
        const restored = new Map();
        let maxZ = 0;
        Object.entries(saved).forEach(([noteId, pos]) => {
          // Only restore positions for notes that still exist
          if (data.some((n) => String(n.id) === String(noteId))) {
            const z = ++maxZ;
            restored.set(String(noteId), { ...pos, zIndex: z });
          }
        });
        if (restored.size > 0) {
          setOpenNotes(restored);
          setNextZIndex(maxZ + 1);
        }
      }
    } catch (err) {
      console.error('Failed to load notes:', err);
    }
  }, [user]);

  const loadGroups = useCallback(async () => {
    try {
      const data = await noteGroupsApi.getAll();
      setGroups(data);
    } catch (err) {
      console.error('Failed to load groups:', err);
    }
  }, []);

  const openNote = useCallback(
    (noteId) => {
      const id = String(noteId);
      setOpenNotes((prev) => {
        if (prev.has(id)) {
          // Already open – bring to front
          const next = new Map(prev);
          setNextZIndex((z) => {
            next.set(id, { ...next.get(id), zIndex: z });
            return z + 1;
          });
          return next;
        }
        const count = prev.size;
        const next = new Map(prev);
        setNextZIndex((z) => {
          next.set(id, {
            x: 50 + count * 30,
            y: 50 + count * 30,
            width: 380,
            height: 320,
            zIndex: z,
          });
          return z + 1;
        });
        return next;
      });
    },
    []
  );

  const closeNote = useCallback((noteId) => {
    const id = String(noteId);
    setOpenNotes((prev) => {
      const next = new Map(prev);
      next.delete(id);
      savePositions(next);
      return next;
    });
  }, []);

  const updateNotePosition = useCallback((noteId, pos) => {
    const id = String(noteId);
    setOpenNotes((prev) => {
      if (!prev.has(id)) return prev;
      const next = new Map(prev);
      const current = next.get(id);
      next.set(id, { ...current, ...pos });
      savePositions(next);
      return next;
    });
  }, []);

  const bringToFront = useCallback((noteId) => {
    const id = String(noteId);
    setOpenNotes((prev) => {
      if (!prev.has(id)) return prev;
      const next = new Map(prev);
      setNextZIndex((z) => {
        next.set(id, { ...next.get(id), zIndex: z });
        return z + 1;
      });
      return next;
    });
  }, []);

  const createNote = useCallback(
    async (data) => {
      const created = await notesApi.create(data);
      setNotes((prev) => [...prev, created]);
      openNote(created.id);
      return created;
    },
    [openNote]
  );

  const updateNote = useCallback(async (id, data) => {
    await notesApi.update(id, data);
    setNotes((prev) =>
      prev.map((n) => (n.id === id ? { ...n, ...data } : n))
    );
  }, []);

  const deleteNote = useCallback(
    async (id) => {
      await notesApi.remove(id);
      setNotes((prev) => prev.filter((n) => n.id !== id));
      closeNote(id);
    },
    [closeNote]
  );

  const searchNotes = useCallback(async (query) => {
    const results = await notesApi.search(query);
    return results;
  }, []);

  const value = {
    notes,
    openNotes,
    groups,
    loadNotes,
    loadGroups,
    openNote,
    closeNote,
    updateNotePosition,
    bringToFront,
    createNote,
    updateNote,
    deleteNote,
    searchNotes,
  };

  return <NotesContext.Provider value={value}>{children}</NotesContext.Provider>;
}

export function useNotes() {
  const ctx = useContext(NotesContext);
  if (!ctx) {
    throw new Error('useNotes must be used within a NotesProvider');
  }
  return ctx;
}
