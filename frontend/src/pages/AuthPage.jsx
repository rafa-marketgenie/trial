import '@fontsource/inter';
import '../styles/index.css';

import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import LoginForm from '../components/auth/LoginForm';
import RegisterForm from '../components/auth/RegisterForm';

/**
 * AuthPage — Full-page authentication screen with login / register toggle.
 * Features a glassmorphism card over animated gradient blobs.
 */
export default function AuthPage() {
  const [mode, setMode] = useState('login');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const { login, register } = useAuth();
  const navigate = useNavigate();

  const handleLogin = async (username, password) => {
    setError(null);
    setLoading(true);
    try {
      await login(username, password);
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Login failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async (username, email, password) => {
    setError(null);
    setLoading(true);
    try {
      await register(username, email, password);
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Registration failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const toggleMode = () => {
    setError(null);
    setMode((prev) => (prev === 'login' ? 'register' : 'login'));
  };

  return (
    <div className="auth-page">
      <div className="auth-container">
        <div className="auth-card">
          <div className="auth-header">
            <h1 className="auth-logo">NoteFlow</h1>
            <p className="auth-subtitle">Your thoughts, beautifully organized</p>
          </div>

          {mode === 'login' ? (
            <LoginForm onSubmit={handleLogin} loading={loading} error={error} />
          ) : (
            <RegisterForm onSubmit={handleRegister} loading={loading} error={error} />
          )}

          <div className="auth-toggle">
            {mode === 'login' ? (
              <>
                Don&apos;t have an account?{' '}
                <button type="button" onClick={toggleMode}>
                  Sign up
                </button>
              </>
            ) : (
              <>
                Already have an account?{' '}
                <button type="button" onClick={toggleMode}>
                  Sign in
                </button>
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
