import Sidebar from './Sidebar';

/**
 * AppLayout — Main application layout wrapper.
 * Renders the sidebar on the left and content area on the right.
 *
 * @param {Object} props
 * @param {React.ReactNode} props.children      - Main content to render beside the sidebar.
 * @param {() => void}      props.onCreateNote  - Callback passed to Sidebar to open create-note modal.
 */
export default function AppLayout({ children, onCreateNote }) {
  return (
    <div className="app-layout">
      <Sidebar onCreateNote={onCreateNote} />
      {children}
    </div>
  );
}
