import client from './client';

export async function getAll() {
  const response = await client.get('/User');
  return response.data;
}

export async function getById(id) {
  const response = await client.get(`/User/${id}`);
  return response.data;
}
