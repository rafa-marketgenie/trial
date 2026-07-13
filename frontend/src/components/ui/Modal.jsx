import { useEffect, useCallback, useRef } from 'react';
import { X } from 'lucide-react';

/**
 * Modal — Overlay modal dialog with glassmorphism card styling.
 *
 * @param {Object} props
 * @param {boolean}  props.isOpen   - Controls visibility of the modal.
 * @param {() => void} props.onClose - Callback fired when the modal requests closing.
 * @param {string}   [props.title]  - Optional title rendered in the modal header.
 * @param {React.ReactNode} props.children - Content rendered inside the modal body.
 * @param {'sm'|'md'|'lg'} [props.size='md'] - Width preset for the modal card.
 */
export default function Modal({ isOpen, onClose, title, children, size = 'md' }) {
  const overlayRef = useRef(null);

  // Close on Escape key
  const handleKeyDown = useCallback(
    (e) => {
      if (e.key === 'Escape') onClose();
    },
    [onClose],
  );

  useEffect(() => {
    if (isOpen) {
      document.addEventListener('keydown', handleKeyDown);
      // Prevent body scroll while modal is open
      document.body.style.overflow = 'hidden';
    }
    return () => {
      document.removeEventListener('keydown', handleKeyDown);
      document.body.style.overflow = '';
    };
  }, [isOpen, handleKeyDown]);

  // Close when clicking the overlay backdrop (not the card itself)
  const handleOverlayClick = (e) => {
    if (e.target === overlayRef.current) onClose();
  };

  if (!isOpen) return null;

  return (
    <div
      className="modal-overlay"
      ref={overlayRef}
      onClick={handleOverlayClick}
      role="dialog"
      aria-modal="true"
      aria-label={title || 'Modal dialog'}
    >
      <div className={`modal-card modal-${size}`}>
        {/* Header */}
        <div className="modal-header">
          {title && <h2 className="modal-title">{title}</h2>}
          <button
            className="modal-close-btn"
            onClick={onClose}
            aria-label="Close modal"
            type="button"
          >
            <X size={20} />
          </button>
        </div>

        {/* Body */}
        <div className="modal-body">{children}</div>
      </div>
    </div>
  );
}
