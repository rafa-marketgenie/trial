/**
 * Input — Dark-themed text input with label, optional icon, and error state.
 *
 * @param {Object} props
 * @param {string}  [props.label]       - Label text displayed above the input.
 * @param {string}  [props.type='text'] - HTML input type.
 * @param {string}  props.value         - Controlled input value.
 * @param {(e: React.ChangeEvent<HTMLInputElement>) => void} props.onChange - Change handler.
 * @param {string}  [props.placeholder] - Placeholder text.
 * @param {string}  [props.error]       - Error message; triggers red border when present.
 * @param {React.ComponentType} [props.icon] - Optional lucide icon *component* rendered on the left.
 * @param {string}  [props.name]        - HTML name attribute.
 * @param {boolean} [props.required]    - Whether the field is required.
 * @param {boolean} [props.autoFocus]   - Whether to auto-focus on mount.
 */
export default function Input({
  label,
  type = 'text',
  value,
  onChange,
  placeholder,
  error,
  icon: Icon,
  name,
  required = false,
  autoFocus = false,
}) {
  return (
    <div className="input-group">
      {label && (
        <label className="input-label" htmlFor={name}>
          {label}
          {required && <span className="input-required">*</span>}
        </label>
      )}

      <div className={`input-wrapper ${error ? 'input-wrapper--error' : ''} ${Icon ? 'input-wrapper--has-icon' : ''}`}>
        {Icon && (
          <span className="input-icon" aria-hidden="true">
            <Icon size={18} />
          </span>
        )}

        <input
          id={name}
          className="input-field"
          type={type}
          name={name}
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          required={required}
          autoFocus={autoFocus}
          aria-invalid={!!error}
          aria-describedby={error ? `${name}-error` : undefined}
        />
      </div>

      {error && (
        <p className="input-error" id={`${name}-error`} role="alert">
          {error}
        </p>
      )}
    </div>
  );
}
