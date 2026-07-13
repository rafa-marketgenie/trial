import { createContext, useContext, useState, useCallback, useEffect, useRef } from 'react';
import { CheckCircle, XCircle, Info, AlertTriangle, X } from 'lucide-react';

/**
 * @typedef {'success'|'error'|'info'|'warning'} ToastType
 *
 * @typedef {Object} ToastItem
 * @property {string}    id        - Unique identifier.
 * @property {string}    message   - Text to display.
 * @property {ToastType} type      - Visual style / icon.
 * @property {boolean}   exiting   - True when the toast is animating out.
 */

/* ------------------------------------------------------------------ */
/*  Context                                                           */
/* ------------------------------------------------------------------ */

const ToastContext = createContext(null);

/**
 * useToast — Access the toast system from any descendant of ToastProvider.
 *
 * @returns {{ addToast: (message: string, type?: ToastType) => void }}
 */
export function useToast() {
  const ctx = useContext(ToastContext);
  if (!ctx) throw new Error('useToast must be used within a <ToastProvider>');
  return ctx;
}

/* ------------------------------------------------------------------ */
/*  Icon map                                                          */
/* ------------------------------------------------------------------ */

const ICON_MAP = {
  success: CheckCircle,
  error: XCircle,
  info: Info,
  warning: AlertTriangle,
};

/* ------------------------------------------------------------------ */
/*  Individual Toast                                                   */
/* ------------------------------------------------------------------ */

const AUTO_DISMISS_MS = 4000;
const EXIT_ANIMATION_MS = 300;

/**
 * Single toast notification bar.
 * @param {Object} props
 * @param {ToastItem} props.toast
 * @param {(id: string) => void} props.onRemove
 */
function ToastItem({ toast, onRemove }) {
  const Icon = ICON_MAP[toast.type] || Info;
  const timerRef = useRef(null);

  // Auto-dismiss
  useEffect(() => {
    timerRef.current = setTimeout(() => onRemove(toast.id), AUTO_DISMISS_MS);
    return () => clearTimeout(timerRef.current);
  }, [toast.id, onRemove]);

  return (
    <div
      className={`toast toast-${toast.type} ${toast.exiting ? 'toast-exit' : 'toast-enter'}`}
      role="status"
      aria-live="polite"
    >
      <Icon size={18} className="toast-icon" />
      <span className="toast-message">{toast.message}</span>
      <button
        className="toast-dismiss"
        onClick={() => onRemove(toast.id)}
        aria-label="Dismiss"
        type="button"
      >
        <X size={14} />
      </button>
    </div>
  );
}

/* ------------------------------------------------------------------ */
/*  Provider                                                          */
/* ------------------------------------------------------------------ */

let toastCounter = 0;

/**
 * ToastProvider — Wraps the app and renders a toast container in the
 * bottom-right corner. Provides `addToast(message, type)` via context.
 *
 * @param {Object} props
 * @param {React.ReactNode} props.children
 */
export function ToastProvider({ children }) {
  const [toasts, setToasts] = useState([]);

  /**
   * Add a toast notification.
   * @param {string} message - Text content.
   * @param {ToastType} [type='info'] - Toast type.
   */
  const addToast = useCallback((message, type = 'info') => {
    const id = `toast-${++toastCounter}`;
    setToasts((prev) => [...prev, { id, message, type, exiting: false }]);
  }, []);

  /**
   * Begin exit animation, then remove the toast from state.
   * @param {string} id
   */
  const removeToast = useCallback((id) => {
    // Mark as exiting to trigger slide-out animation
    setToasts((prev) =>
      prev.map((t) => (t.id === id ? { ...t, exiting: true } : t)),
    );
    // Remove from DOM after animation completes
    setTimeout(() => {
      setToasts((prev) => prev.filter((t) => t.id !== id));
    }, EXIT_ANIMATION_MS);
  }, []);

  return (
    <ToastContext.Provider value={{ addToast }}>
      {children}

      {/* Toast stack — bottom-right, newest at the bottom */}
      {toasts.length > 0 && (
        <div className="toast-container" aria-label="Notifications">
          {toasts.map((toast) => (
            <ToastItem key={toast.id} toast={toast} onRemove={removeToast} />
          ))}
        </div>
      )}
    </ToastContext.Provider>
  );
}

export default ToastProvider;
