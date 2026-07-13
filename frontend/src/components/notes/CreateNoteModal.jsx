import { useState, useCallback } from 'react';
import { Type } from 'lucide-react';
import Modal from '../ui/Modal';
import Input from '../ui/Input';
import Button from '../ui/Button';
import { useNotes } from '../../contexts/NotesContext';

/**
 * CreateNoteModal — Modal form for creating a new note.
 *
 * @param {Object}  props
 * @param {boolean} props.isOpen  - Controls visibility.
 * @param {() => void} props.onClose - Callback to close the modal.
 */
export default function CreateNoteModal({ isOpen, onClose }) {
  const { createNote, groups } = useNotes();

  const [title, setTitle] = useState('');
  const [groupId, setGroupId] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const resetForm = useCallback(() => {
    setTitle('');
    setGroupId('');
    setError('');
  }, []);

  const handleClose = useCallback(() => {
    resetForm();
    onClose();
  }, [onClose, resetForm]);

  const handleSubmit = useCallback(
    async (e) => {
      e.preventDefault();

      const trimmedTitle = title.trim();
      if (!trimmedTitle) {
        setError('Title is required');
        return;
      }

      setLoading(true);
      setError('');

      try {
        const payload = { title: trimmedTitle, content: ' ' };
        if (groupId) payload.groupId = Number(groupId);

        await createNote(payload);
        resetForm();
        onClose();
      } catch (err) {
        setError(err?.response?.data?.message || 'Failed to create note');
      } finally {
        setLoading(false);
      }
    },
    [title, groupId, createNote, onClose, resetForm],
  );

  return (
    <Modal isOpen={isOpen} onClose={handleClose} title="New Note" size="sm">
      <form className="create-note-form" onSubmit={handleSubmit}>
        <Input
          label="Title"
          name="title"
          placeholder="Untitled note"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          icon={Type}
          required
          autoFocus
          error={error}
        />

        {groups.length > 0 && (
          <div className="input-group">
            <label className="input-label" htmlFor="create-note-group">
              Group
            </label>
            <select
              id="create-note-group"
              className="create-note-group-select"
              value={groupId}
              onChange={(e) => setGroupId(e.target.value)}
            >
              <option value="">No group</option>
              {groups.map((g) => (
                <option key={g.id} value={g.id}>
                  {g.name}
                </option>
              ))}
            </select>
          </div>
        )}

        <Button type="submit" loading={loading}>
          Create Note
        </Button>
      </form>
    </Modal>
  );
}
