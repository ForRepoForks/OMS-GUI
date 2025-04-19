import { test, expect } from 'vitest';
import api from './api';

test('GET /api/products returns data', async () => {
  try {
    const response = await api.get('/api/products');
    expect(response.status).toBe(200);
    expect(response.data).toBeDefined();
    // Optionally, add more assertions based on expected data shape
  } catch (error: any) {
    // Optionally, improve error reporting
    throw new Error(
      'API test error: ' + (error.response?.data ? JSON.stringify(error.response.data) : error.message)
    );
  }
});
