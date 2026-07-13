import { createContext, useContext, useState, useEffect, useCallback } from 'react';
import * as authApi from '../api/auth';

const AuthContext = createContext(null);

function decodeToken(token) {
  try {
    const payload = token.split('.')[1];
    // Base64url → Base64, then decode
    const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
    const json = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    const claims = JSON.parse(json);

    const userId =
      claims.nameid ||
      claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ||
      null;
    const username =
      claims.unique_name ||
      claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
      null;

    return { id: userId, username };
  } catch {
    return null;
  }
}

export function AuthProvider({ children }) {
  const [token, setToken] = useState(null);
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // On mount, restore token from localStorage
  useEffect(() => {
    const stored = localStorage.getItem('auth_token');
    if (stored) {
      const decoded = decodeToken(stored);
      if (decoded) {
        setToken(stored);
        setUser(decoded);
      } else {
        localStorage.removeItem('auth_token');
      }
    }
    setLoading(false);
  }, []);

  const login = useCallback(async (username, password) => {
    const data = await authApi.login(username, password);
    const jwt = data.token;
    localStorage.setItem('auth_token', jwt);
    setToken(jwt);
    setUser(decodeToken(jwt));
  }, []);

  const register = useCallback(async (username, email, password) => {
    const data = await authApi.register(username, email, password);
    const jwt = data.token;
    localStorage.setItem('auth_token', jwt);
    setToken(jwt);
    setUser(decodeToken(jwt));
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('note_positions');
    setToken(null);
    setUser(null);
    window.location.href = '/auth';
  }, []);

  const value = {
    token,
    user,
    isAuthenticated: !!token && !!user,
    loading,
    login,
    register,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return ctx;
}
