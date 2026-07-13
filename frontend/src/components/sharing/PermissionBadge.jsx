import { Eye, Pencil } from 'lucide-react';

/**
 * PermissionBadge — Small inline badge showing a permission type.
 *
 * @param {Object} props
 * @param {'View'|'Edit'} props.type - The permission level to display.
 */
export default function PermissionBadge({ type }) {
  const isView = type === 'View';

  return (
    <span className={`permission-badge ${isView ? 'view' : 'edit'}`}>
      {isView ? <Eye size={14} /> : <Pencil size={14} />}
      {type}
    </span>
  );
}
