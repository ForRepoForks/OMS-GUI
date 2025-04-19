import * as React from 'react';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import Stack from '@mui/material/Stack';
import api from '../api';

export type Product = {
  id: number;
  name: string;
  price: number;
  discountPercentage?: number | null;
  discountQuantityThreshold?: number | null;
};

export default function Products() {
  const [products, setProducts] = React.useState<Product[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);

  React.useEffect(() => {
    setLoading(true);
    setError(null);
    api
      .get('/api/products?page=1&pageSize=50')
      .then((res) => {
        const items = res.data.items || res.data;
        setProducts(items);
      })
      .catch((_err) => {
        setError('Failed to load products');
      })
      .finally(() => setLoading(false));
  }, []);

  const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', width: 70 },
    { field: 'name', headerName: 'Name', flex: 1 },
    {
      field: 'price',
      headerName: 'Price',
      type: 'number',
      width: 120,
      valueFormatter: (params: { value: any }) => {
        const value = params?.value;
        return value != null ? `$${value}` : '-';
      },
    },
    {
      field: 'discountPercentage',
      headerName: 'Discount %',
      width: 120,
      valueFormatter: (params: { value: any }) => {
        const value = params?.value;
        return value != null && value !== '' ? `${value}%` : '-';
      },
    },
    {
      field: 'discountQuantityThreshold',
      headerName: 'Discount Qty Threshold',
      width: 170,
      valueFormatter: (params: { value: any }) => {
        const value = params?.value;
        return value != null && value !== '' ? value : '-';
      },
    },
  ];

  return (
    <Stack spacing={2}>
      <h2 style={{ margin: 0 }}>Products</h2>
      <div
        style={{
          height: 600,
          width: '100%',
          background: '#fff',
          borderRadius: 8,
          position: 'relative',
        }}
      >
        {loading ? (
          <div
            style={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              height: '100%',
            }}
          >
            <span>Loading...</span>
          </div>
        ) : error ? (
          <div
            style={{
              color: 'red',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              height: '100%',
            }}
          >
            {error}
          </div>
        ) : (
          <DataGrid
            rows={products}
            columns={columns}
            pageSizeOptions={[10, 25, 50]}
            initialState={{
              pagination: {
                paginationModel: { pageSize: 10, page: 0 },
              },
            }}
            disableRowSelectionOnClick
          />
        )}
      </div>
    </Stack>
  );
}
