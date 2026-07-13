import { useState } from 'react';
import { User, Mail, Lock } from 'lucide-react';
import Input from '../ui/Input';
import Button from '../ui/Button';

/**
 * RegisterForm — Username + Email + Password registration form with client-side validation.
 *
 * @param {Object} props
 * @param {(username: string, email: string, password: string) => void} props.onSubmit - Called with credentials on submit.
 * @param {boolean} [props.loading=false] - Disables the form and shows a spinner.
 * @param {string|null} [props.error] - Server-side error message.
 */
export default function RegisterForm({ onSubmit, loading = false, error = null }) {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [fieldErrors, setFieldErrors] = useState({});

  const validate = () => {
    const errors = {};

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      errors.email = 'Please enter a valid email address';
    }

    if (password.length < 8) {
      errors.password = 'Password must be at least 8 characters';
    }

    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validate()) {
      onSubmit(username, email, password);
    }
  };

  return (
    <form className="auth-form" onSubmit={handleSubmit}>
      <Input
        label="Username"
        name="username"
        placeholder="Choose a username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        icon={User}
        required
        autoFocus
      />

      <Input
        label="Email"
        name="email"
        type="email"
        placeholder="you@example.com"
        value={email}
        onChange={(e) => {
          setEmail(e.target.value);
          if (fieldErrors.email) {
            setFieldErrors((prev) => ({ ...prev, email: undefined }));
          }
        }}
        icon={Mail}
        error={fieldErrors.email}
        required
      />

      <Input
        label="Password"
        name="password"
        type="password"
        placeholder="At least 8 characters"
        value={password}
        onChange={(e) => {
          setPassword(e.target.value);
          if (fieldErrors.password) {
            setFieldErrors((prev) => ({ ...prev, password: undefined }));
          }
        }}
        icon={Lock}
        error={fieldErrors.password}
        required
      />

      {error && <div className="auth-error">{error}</div>}

      <Button type="submit" loading={loading} size="lg">
        Create Account
      </Button>
    </form>
  );
}
