import { useEffect, useRef } from 'react';
import { Search, X } from 'lucide-react';

/**
 * SearchBar — Debounced search input with search icon and clear button.
 *
 * @param {Object} props
 * @param {string}  props.value       - Controlled input value.
 * @param {(value: string) => void} props.onChange  - Called on every keystroke (updates value).
 * @param {(query: string) => void} props.onSearch  - Called after 300 ms of inactivity with the current value.
 * @param {string}  [props.placeholder='Search notes…'] - Placeholder text.
 */
export default function SearchBar({
  value,
  onChange,
  onSearch,
  placeholder = 'Search notes…',
}) {
  const debounceRef = useRef(null);

  // Debounce: fire onSearch 300 ms after the user stops typing
  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);

    debounceRef.current = setTimeout(() => {
      onSearch(value);
    }, 300);

    return () => clearTimeout(debounceRef.current);
  }, [value, onSearch]);

  /** Handle input change */
  const handleChange = (e) => {
    onChange(e.target.value);
  };

  /** Clear the search field and notify parent */
  const handleClear = () => {
    onChange('');
    onSearch('');
  };

  return (
    <div className="search-bar">
      <Search size={18} className="search-icon" aria-hidden="true" />

      <input
        className="search-input"
        type="text"
        value={value}
        onChange={handleChange}
        placeholder={placeholder}
        aria-label="Search"
      />

      {value && (
        <button
          className="search-clear"
          onClick={handleClear}
          aria-label="Clear search"
          type="button"
        >
          <X size={16} />
        </button>
      )}
    </div>
  );
}
