import { useState } from 'react';
import { User, Lock } from 'lucide-react';
import Input from '../ui/Input';
import Button from '../ui/Button';

/**
 * LoginForm — Username + Password login form.
 *
 * @param {Object} props
 * @param {(username: string, password: string) => void} props.onSubmit - Called with credentials on submit.
 * @param {boolean} [props.loading=false] - Disables the form and shows a spinner.
 * @param {string|null} [props.error] - Error message displayed above the submit button.
 */
export default function LoginForm({ onSubmit, loading = false, error = null }) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(username, password);
  };

  return (
    <form className="auth-form" onSubmit={handleSubmit}>
      <Input
        label="Username"
        name="username"
        placeholder="Enter your username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        icon={User}
        required
        autoFocus
      />

      <Input
        label="Password"
        name="password"
        type="password"
        placeholder="Enter your password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        icon={Lock}
        required
      />

      {error && <div className="auth-error">{error}</div>}

      <Button type="submit" loading={loading} size="lg">
        Sign In
      </Button>
    </form>
  );
}
