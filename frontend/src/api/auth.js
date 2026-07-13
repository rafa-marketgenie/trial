import client from './client';

export async function login(username, password) {
  const response = await client.post('/Auth/login', {
    Username: username,
    Password: password,
  });
  return response.data;
}

export async function register(username, email, password) {
  const response = await client.post('/Auth/register', {
    Username: username,
    Email: email,
    Password: password,
  });
  return response.data;
}
