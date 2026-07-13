import { useState, useCallback, useRef } from 'react';
import { Rnd } from 'react-rnd';
import { Share2, Trash2, X, GripHorizontal } from 'lucide-react';
import NoteEditor from './NoteEditor';

/**
 * Compute a human-friendly relative time string such as "3 min ago".
 */
function timeAgo(dateString) {
  if (!dateString) return '';
  const seconds = Math.floor((Date.now() - new Date(dateString).getTime()) / 1000);
  if (seconds < 5) return 'just now';
  if (seconds < 60) return `${seconds}s ago`;
  const minutes = Math.floor(seconds / 60);
  if (minutes < 60) return `${minutes} min ago`;
  const hours = Math.floor(minutes / 60);
  if (hours < 24) return `${hours}h ago`;
  const days = Math.floor(hours / 24);
  return `${days}d ago`;
}

/**
 * NoteCard — Floating, draggable & resizable note window on the canvas.
 */
export default function NoteCard({
  note,
  position,
  isActive,
  onFocus,
  onPositionChange,
  onUpdate,
  onDelete,
  onShare,
  onClose,
  groupColor,
}) {
  const [title, setTitle] = useState(note.title ?? '');
  const titleRef = useRef(null);

  // ── Drag / Resize handlers ──
  const handleDragStop = useCallback(
    (_e, d) => {
      onPositionChange?.(note.id, { x: d.x, y: d.y });
    },
    [note.id, onPositionChange],
  );

  const handleResizeStop = useCallback(
    (_e, _dir, ref, _delta, pos) => {
      onPositionChange?.(note.id, {
        x: pos.x,
        y: pos.y,
        width: parseInt(ref.style.width, 10),
        height: parseInt(ref.style.height, 10),
      });
    },
    [note.id, onPositionChange],
  );

  // ── Title editing ──
  const handleTitleBlur = useCallback(() => {
    const trimmed = title.trim();
    if (trimmed && trimmed !== note.title) {
      onUpdate?.(note.id, { title: trimmed, content: note.content });
    }
  }, [title, note.id, note.title, note.content, onUpdate]);

  // ── Content editing (debounced by NoteEditor) ──
  const handleContentChange = useCallback(
    (newContent) => {
      onUpdate?.(note.id, { title: note.title, content: newContent });
    },
    [note.id, note.title, onUpdate],
  );

  const handleTitleChange = useCallback(
    (e) => {
      const newTitle = e.target.value;
      setTitle(newTitle);
      onUpdate?.(note.id, {title: newTitle, content: note.content});
    },
    [note.id, note.content, onUpdate],
  );

  // ── Actions ──
  const handleDelete = useCallback(() => {
    if (window.confirm('Delete this note? This action cannot be undone.')) {
      onDelete?.(note.id);
    }
  }, [note.id, onDelete]);

  const handleShare = useCallback(() => {
    onShare?.(note);
  }, [note, onShare]);

  const handleClose = useCallback(() => {
    onClose?.(note.id);
  }, [note.id, onClose]);

  // ── Colour bar style ──
  const colorBarStyle = groupColor
    ? { background: groupColor }
    : { background: 'var(--accent-gradient)' };

  return (
    <Rnd
      default={{
        x: position.x,
        y: position.y,
        width: position.width,
        height: position.height,
      }}
      minWidth={280}
      minHeight={220}
      bounds="parent"
      dragHandleClassName="note-drag-handle"
      onDragStart={() => onFocus?.(note.id)}
      onDragStop={handleDragStop}
      onResizeStop={handleResizeStop}
      style={{ zIndex: position.zIndex }}
      enableResizing={{
        top: true,
        right: true,
        bottom: true,
        left: true,
        topRight: true,
        topLeft: true,
        bottomRight: true,
        bottomLeft: true,
      }}
    >
      <div
        className={`note-card${isActive ? ' note-card-active' : ''}`}
        onMouseDown={() => onFocus?.(note.id)}
      >
        {/* Colour bar */}
        <div className="note-card-color-bar" style={colorBarStyle} />

        {/* Header */}
        <div className="note-card-header">
          {/* Dedicated drag grip — this is the actual drag handle */}
          <div className="note-drag-handle" title="Drag to move">
            <GripHorizontal size={16} />
          </div>

          <input
            ref={titleRef}
            className="note-card-title"
            value={title}
            // onChange={(e) => setTitle(e.target.value)}
            onChange={handleTitleChange}
            onBlur={handleTitleBlur}
            spellCheck={false}
            placeholder="Untitled"
          />

          <div className="note-card-actions">
            <button
              className="note-card-action"
              onClick={handleShare}
              title="Share"
              type="button"
            >
              <Share2 size={14} />
            </button>
            <button
              className="note-card-action danger"
              onClick={handleDelete}
              title="Delete"
              type="button"
            >
              <Trash2 size={14} />
            </button>
            <button
              className="note-card-action"
              onClick={handleClose}
              title="Close"
              type="button"
            >
              <X size={14} />
            </button>
          </div>
        </div>

        {/* Body */}
        <div className="note-card-body">
          <NoteEditor
            content={note.content}
            onChange={handleContentChange}
          />
        </div>

        {/* Footer */}
        <div className="note-card-footer">
          <span className="note-card-meta">
            Updated {timeAgo(note.updatedAt)}
          </span>
          <div className="note-card-badges" />
        </div>
      </div>
    </Rnd>
  );
}
