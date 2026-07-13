import { Loader2 } from 'lucide-react';

/**
 * Button — Themed button with variant, size, icon, and loading support.
 *
 * @param {Object} props
 * @param {React.ReactNode}  props.children  - Button label / content.
 * @param {'primary'|'secondary'|'danger'|'ghost'} [props.variant='primary'] - Visual style.
 * @param {'sm'|'md'|'lg'} [props.size='md'] - Size preset.
 * @param {React.ReactNode}  [props.icon]    - Optional leading icon element.
 * @param {boolean} [props.loading=false]     - Shows a spinner and disables the button.
 * @param {boolean} [props.disabled=false]    - Disables the button.
 * @param {() => void} [props.onClick]        - Click handler.
 * @param {string}  [props.type='button']     - HTML button type attribute.
 * @param {string}  [props.className]         - Additional CSS classes.
 */
export default function Button({
  children,
  variant = 'primary',
  size = 'md',
  icon,
  loading = false,
  disabled = false,
  onClick,
  type = 'button',
  className = '',
  ...rest
}) {
  const classes = [
    'btn',
    `btn-${variant}`,
    `btn-${size}`,
    loading ? 'btn-loading' : '',
    className,
  ]
    .filter(Boolean)
    .join(' ');

  return (
    <button
      className={classes}
      type={type}
      disabled={disabled || loading}
      onClick={onClick}
      {...rest}
    >
      {/* Spinner replaces the icon when loading */}
      {loading ? (
        <Loader2 size={size === 'sm' ? 14 : size === 'lg' ? 20 : 16} className="btn-spinner" />
      ) : (
        icon && <span className="btn-icon">{icon}</span>
      )}
      <span className="btn-label">{children}</span>
    </button>
  );
}
