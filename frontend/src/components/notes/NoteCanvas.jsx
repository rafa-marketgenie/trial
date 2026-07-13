import { useState, useCallback } from 'react';
import { StickyNote } from 'lucide-react';
import { useNotes } from '../../contexts/NotesContext';
import NoteCard from './NoteCard';
import ShareModal from '../sharing/ShareModal';

/**
 * NoteCanvas — The primary workspace area where notes float as draggable cards.
 */
export default function NoteCanvas() {
  const {
    notes,
    openNotes,
    groups,
    bringToFront,
    updateNotePosition,
    updateNote,
    deleteNote,
    closeNote,
  } = useNotes();

  const [shareNoteId, setShareNoteId] = useState(null);

  // ── Handlers ──
  const handleFocus = useCallback(
    (noteId) => bringToFront(noteId),
    [bringToFront],
  );

  const handlePositionChange = useCallback(
    (noteId, newPos) => updateNotePosition(noteId, newPos),
    [updateNotePosition],
  );

  const handleUpdate = useCallback(
    (noteId, data) => updateNote(noteId, data),
    [updateNote],
  );

  const handleDelete = useCallback(
    (noteId) => deleteNote(noteId),
    [deleteNote],
  );

  const handleShare = useCallback((note) => {
    setShareNoteId(note.id);
  }, []);

  const handleClose = useCallback(
    (noteId) => closeNote(noteId),
    [closeNote],
  );

  // Resolve a note's group colour
  const getGroupColor = useCallback(
    (groupId) => {
      if (!groupId) return undefined;
      const group = groups.find((g) => g.id === groupId);
      return group?.color;
    },
    [groups],
  );

  // Build array of open note entries
  const openEntries = [];
  openNotes.forEach((position, noteId) => {
    const note = notes.find((n) => String(n.id) === String(noteId));
    if (note) openEntries.push({ note, position });
  });

  // Determine which note is on top (highest zIndex)
  const maxZ = openEntries.reduce(
    (max, e) => Math.max(max, e.position.zIndex ?? 0),
    0,
  );

  return (
    <div className="note-canvas">
      {openEntries.length === 0 ? (
        <div className="note-canvas-empty">
          <div className="note-canvas-empty-icon">
            <StickyNote size={56} />
          </div>
          <h3>No notes open</h3>
          <p>Click a note in the sidebar or create a new one</p>
        </div>
      ) : (
        openEntries.map(({ note, position }) => (
          <NoteCard
            key={note.id}
            note={note}
            position={position}
            isActive={position.zIndex === maxZ}
            onFocus={handleFocus}
            onPositionChange={handlePositionChange}
            onUpdate={handleUpdate}
            onDelete={handleDelete}
            onShare={handleShare}
            onClose={handleClose}
            groupColor={getGroupColor(note.groupId)}
          />
        ))
      )}

      {/* Share Modal */}
      {shareNoteId != null && (
        <ShareModal
          isOpen
          noteId={shareNoteId}
          onClose={() => setShareNoteId(null)}
        />
      )}
    </div>
  );
}
