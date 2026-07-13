import client from './client';

export async function getById(id) {
  const response = await client.get(`/PermissionPolicy/${id}`);
  return response.data;
}

export async function grant(data) {
  const response = await client.post('/PermissionPolicy', data);
  return response.data;
}

export async function update(id, data) {
  await client.put(`/PermissionPolicy/${id}`, data);
}

export async function remove(id) {
  await client.delete(`/PermissionPolicy/${id}`);
}
