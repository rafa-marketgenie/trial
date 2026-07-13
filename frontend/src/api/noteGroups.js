import client from './client';

export async function getAll() {
  const response = await client.get('/NoteGroup');
  return response.data;
}

export async function getById(id) {
  const response = await client.get(`/NoteGroup/${id}`);
  return response.data;
}

export async function create(data) {
  const response = await client.post('/NoteGroup', data);
  return response.data;
}

export async function update(id, data) {
  await client.put(`/NoteGroup/${id}`, data);
}

export async function remove(id) {
  await client.delete(`/NoteGroup/${id}`);
}
