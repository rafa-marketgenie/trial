import { useState, useEffect, useRef, useCallback } from 'react';

const DEBOUNCE_MS = 1500;

/**
 * NoteEditor — Textarea-based editor for note content.
 *
 * @param {Object} props
 * @param {string}  props.content   - Current note content value.
 * @param {(value: string) => void} props.onChange - Callback fired after debounce with new content.
 * @param {boolean} [props.readOnly=false] - Disables editing when true.
 */
export default function NoteEditor({ content, onChange, readOnly = false }) {
  const [localContent, setLocalContent] = useState(content ?? '');
  const [isSaving, setIsSaving] = useState(false);
  const timerRef = useRef(null);

  // Sync local state when the parent provides new content (e.g. after a server round-trip)
  useEffect(() => {
    setLocalContent(content ?? '');
  }, [content]);

  // Cleanup on unmount
  useEffect(() => {
    return () => {
      if (timerRef.current) clearTimeout(timerRef.current);
    };
  }, []);

  const handleChange = useCallback(
    (e) => {
      const value = e.target.value;
      setLocalContent(value);

      // Clear previous debounce timer
      if (timerRef.current) clearTimeout(timerRef.current);

      // Show saving indicator
      setIsSaving(true);

      // Set up new debounce timer
      timerRef.current = setTimeout(() => {
        onChange?.(value);
        setIsSaving(false);
        timerRef.current = null;
      }, DEBOUNCE_MS);
    },
    [onChange],
  );

  return (
    <div className="note-editor">
      <textarea
        value={localContent}
        onChange={handleChange}
        placeholder="Start writing..."
        readOnly={readOnly}
        spellCheck
      />
      {isSaving && (
        <span className="note-card-saving">Saving…</span>
      )}
    </div>
  );
}
