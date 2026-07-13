import client from './client';

export async function getAll() {
  const response = await client.get('/Notes');
  return response.data;
}

export async function getById(id) {
  const response = await client.get(`/Notes/${id}`);
  return response.data;
}

export async function getByUser(userId) {
  const response = await client.get(`/Notes/user/${userId}`);
  return response.data;
}

export async function search(query) {
  const response = await client.get('/Notes/search', {
    params: { query },
  });
  return response.data;
}

export async function create(data) {
  const response = await client.post('/Notes', data);
  return response.data;
}

export async function update(id, data) {
  await client.put(`/Notes/${id}`, data);
}

export async function remove(id) {
  await client.delete(`/Notes/${id}`);
}
