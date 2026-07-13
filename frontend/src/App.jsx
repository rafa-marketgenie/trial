import { Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider, useAuth } from './contexts/AuthContext'
import { NotesProvider } from './contexts/NotesContext'
import { ToastProvider } from './components/ui/Toast'
import AuthPage from './pages/AuthPage'
import WorkspacePage from './pages/WorkspacePage'

/**
 * Protected route wrapper — redirects to /auth if not authenticated
 */
function ProtectedRoute({ children }) {
  const { isAuthenticated, loading } = useAuth()

  if (loading) {
    return (
      <div style={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        height: '100vh',
        background: 'var(--bg-deep)',
        color: 'var(--text-secondary)',
        fontFamily: 'var(--font-family)',
      }}>
        <div className="btn-spinner" style={{ width: 32, height: 32, borderWidth: 3, borderTopColor: 'var(--accent-primary)' }} />
      </div>
    )
  }

  return isAuthenticated ? children : <Navigate to="/auth" replace />
}

/**
 * Public route wrapper — redirects to / if already authenticated
 */
function PublicRoute({ children }) {
  const { isAuthenticated, loading } = useAuth()

  if (loading) return null
  return isAuthenticated ? <Navigate to="/" replace /> : children
}

function App() {
  return (
    <AuthProvider>
      <ToastProvider>
        <Routes>
          <Route
            path="/auth"
            element={
              <PublicRoute>
                <AuthPage />
              </PublicRoute>
            }
          />
          <Route
            path="/*"
            element={
              <ProtectedRoute>
                <NotesProvider>
                  <WorkspacePage />
                </NotesProvider>
              </ProtectedRoute>
            }
          />
        </Routes>
      </ToastProvider>
    </AuthProvider>
  )
}

export default App
