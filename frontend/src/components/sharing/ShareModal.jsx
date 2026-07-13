import { useState, useEffect, useCallback } from 'react';
import Modal from '../ui/Modal';
import Button from '../ui/Button';
import { useToast } from '../ui/Toast';
import { useAuth } from '../../contexts/AuthContext';
import { getAll } from '../../api/users';
import { grant, remove } from '../../api/permissions';
import { Search } from 'lucide-react';

/**
 * ShareModal — Modal dialog for sharing a note with other users.
 *
 * @param {Object} props
 * @param {boolean}  props.isOpen  - Controls visibility.
 * @param {() => void} props.onClose - Callback to close the modal.
 * @param {string|number} props.noteId - ID of the note being shared.
 */
export default function ShareModal({ isOpen, onClose, noteId }) {
  const { user } = useAuth();
  const { addToast } = useToast();

  const [users, setUsers] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedUser, setSelectedUser] = useState(null);
  const [permissionType, setPermissionType] = useState('View');
  const [loading, setLoading] = useState(false);

  // Fetch all users (excluding current) when modal opens
  useEffect(() => {
    if (!isOpen) return;

    let cancelled = false;

    async function fetchUsers() {
      try {
        const allUsers = await getAll();
        if (!cancelled) {
          setUsers(allUsers.filter((u) => String(u.id) !== String(user?.id)));
        }
      } catch (err) {
        if (!cancelled) {
          addToast('Failed to load users', 'error');
        }
      }
    }

    fetchUsers();

    // Reset state on open
    setSearchQuery('');
    setSelectedUser(null);
    setPermissionType('View');

    return () => {
      cancelled = true;
    };
  }, [isOpen, user?.id, addToast]);

  // Filter users by search query
  const filteredUsers = users.filter((u) => {
    if (!searchQuery) return true;
    const q = searchQuery.toLowerCase();
    return (
      u.username.toLowerCase().includes(q) ||
      u.email.toLowerCase().includes(q)
    );
  });

  const handleShare = useCallback(async () => {
    if (!selectedUser) {
      addToast('Please select a user to share with', 'warning');
      return;
    }

    setLoading(true);
    try {
      await grant({
        GuestId: selectedUser.id,
        NoteId: noteId,
        PermissionType: permissionType,
      });
      addToast(`Shared with ${selectedUser.username} (${permissionType})`, 'success');
      setSelectedUser(null);
    } catch (err) {
      addToast('Failed to share note', 'error');
    } finally {
      setLoading(false);
    }
  }, [selectedUser, noteId, permissionType, addToast]);

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Share Note" size="md">
      {/* Search */}
      <div className="share-modal-search">
        <Search size={16} aria-hidden="true" />
        <input
          type="text"
          placeholder="Search users by name or email…"
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
      </div>

      {/* User list */}
      <div className="share-section-title">Select a user</div>
      <div className="share-user-list">
        {filteredUsers.length === 0 && (
          <p className="share-user-list-empty">No users found</p>
        )}
        {filteredUsers.map((u) => (
          <div
            key={u.id}
            className={`share-user-item ${
              selectedUser?.id === u.id ? 'selected' : ''
            }`}
            onClick={() => setSelectedUser(u)}
          >
            <div className="share-user-avatar">
              {u.username.charAt(0).toUpperCase()}
            </div>
            <div className="share-user-info">
              <span className="share-user-name">{u.username}</span>
              <span className="share-user-email">{u.email}</span>
            </div>
          </div>
        ))}
      </div>

      {/* Permission type toggle */}
      <div className="share-section-title">Permission</div>
      <div className="share-permission-select">
        <button
          type="button"
          className={`share-permission-option ${permissionType === 'View' ? 'active' : ''}`}
          onClick={() => setPermissionType('View')}
        >
          Can View
        </button>
        <button
          type="button"
          className={`share-permission-option ${permissionType === 'Edit' ? 'active' : ''}`}
          onClick={() => setPermissionType('Edit')}
        >
          Can Edit
        </button>
      </div>

      {/* Share action */}
      <Button
        variant="primary"
        onClick={handleShare}
        loading={loading}
        disabled={!selectedUser}
        className="share-submit-btn"
      >
        Share
      </Button>
    </Modal>
  );
}
