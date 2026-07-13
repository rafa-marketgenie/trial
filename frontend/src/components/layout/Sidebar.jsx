import { useState, useCallback } from 'react';
import { Plus, LogOut, StickyNote } from 'lucide-react';
import SearchBar from '../ui/SearchBar';
import { useNotes } from '../../contexts/NotesContext';
import { useAuth } from '../../contexts/AuthContext';

/**
 * Sidebar — Left-hand navigation panel.
 *
 * @param {Object} props
 * @param {() => void} props.onCreateNote - Callback to open the create-note modal.
 */
export default function Sidebar({ onCreateNote }) {
  const { notes, groups, openNotes, openNote, searchNotes } = useNotes();
  const { user, logout } = useAuth();

  const [searchValue, setSearchValue] = useState('');
  const [searchResults, setSearchResults] = useState(null);
  const [activeGroupId, setActiveGroupId] = useState(null);

  // Search handler – called after debounce by SearchBar
  const handleSearch = useCallback(
    async (query) => {
      if (!query.trim()) {
        setSearchResults(null);
        return;
      }
      try {
        const results = await searchNotes(query);
        setSearchResults(results);
      } catch {
        setSearchResults([]);
      }
    },
    [searchNotes],
  );

  // Click on a group to filter notes, click again to clear
  const handleGroupClick = (groupId) => {
    setActiveGroupId((prev) => (prev === groupId ? null : groupId));
    setSearchValue('');
    setSearchResults(null);
  };

  // Derive the note list to display
  let displayedNotes = searchResults ?? notes;
  if (activeGroupId && !searchResults) {
    displayedNotes = displayedNotes.filter(
      (n) => String(n.groupId) === String(activeGroupId),
    );
  }

  // Helper: find group color for a note
  const groupColorFor = (note) => {
    if (!note.groupId) return 'transparent';
    const group = groups.find((g) => String(g.id) === String(note.groupId));
    return group?.color ?? 'transparent';
  };

  // Format date for display
  const formatDate = (dateStr) => {
    if (!dateStr) return '';
    const d = new Date(dateStr);
    return d.toLocaleDateString(undefined, {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
    });
  };

  return (
    <aside className="sidebar">
      {/* Brand header */}
      <div className="sidebar-header">
        <div className="sidebar-brand">
          <StickyNote size={22} style={{ color: 'var(--accent-primary)' }} />
          <h1>NoteFlow</h1>
        </div>
      </div>

      {/* Search */}
      <div className="sidebar-search">
        <SearchBar
          value={searchValue}
          onChange={setSearchValue}
          onSearch={handleSearch}
          placeholder="Search notes…"
        />
      </div>

      {/* Scrollable content area */}
      <div className="sidebar-content">
        {/* My Notes */}
        <div className="sidebar-section">
          <div className="sidebar-section-header">
            <span className="sidebar-section-title">My Notes</span>
            <button
              className="sidebar-section-action"
              onClick={onCreateNote}
              aria-label="Create new note"
              type="button"
            >
              <Plus size={16} />
            </button>
          </div>

          {displayedNotes.length === 0 ? (
            <p className="note-list-empty">
              {searchResults
                ? 'No matching notes found'
                : 'No notes yet. Create your first one!'}
            </p>
          ) : (
            <ul className="note-list">
              {displayedNotes.map((note) => {
                const isOpen = openNotes.has(String(note.id));
                return (
                  <li
                    key={note.id}
                    className={`note-list-item${isOpen ? ' active' : ''}`}
                    onClick={() => openNote(note.id)}
                  >
                    <span
                      className="note-list-item-color"
                      style={{ backgroundColor: groupColorFor(note) }}
                    />
                    <div className="note-list-item-content">
                      <span className="note-list-item-title">
                        {note.title || 'Untitled'}
                      </span>
                      <span className="note-list-item-meta">
                        {formatDate(note.updatedAt || note.createdAt)}
                      </span>
                    </div>
                  </li>
                );
              })}
            </ul>
          )}
        </div>

        {/* Groups */}
        {groups.length > 0 && (
          <div className="sidebar-section">
            <div className="sidebar-section-header">
              <span className="sidebar-section-title">Groups</span>
            </div>
            <ul className="group-list">
              {groups.map((group) => (
                <li
                  key={group.id}
                  className={`group-item${activeGroupId === group.id ? ' active' : ''}`}
                  onClick={() => handleGroupClick(group.id)}
                >
                  <span
                    className="group-color-dot"
                    style={{ backgroundColor: group.color }}
                  />
                  <span className="group-name">{group.name}</span>
                </li>
              ))}
            </ul>
          </div>
        )}
      </div>

      {/* Footer — user info + logout */}
      <div className="sidebar-footer">
        <div className="sidebar-user">
          <div className="sidebar-avatar">
            {user?.username?.charAt(0).toUpperCase() ?? '?'}
          </div>
          <span className="sidebar-username">{user?.username ?? 'User'}</span>
          <button
            className="sidebar-logout"
            onClick={logout}
            aria-label="Log out"
            type="button"
          >
            <LogOut size={18} />
          </button>
        </div>
      </div>
    </aside>
  );
}
