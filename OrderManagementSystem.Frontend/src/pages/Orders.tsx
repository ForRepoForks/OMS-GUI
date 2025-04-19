import * as React from 'react';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import OrderDialog from '../components/OrderDialog';
import OrderDetailsDialog from '../components/OrderDetailsDialog';

import api from '../api';

export type Order = { id?: number; customer: string; product: string; quantity: number };

export default function Orders() {
  const [orders, setOrders] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);
  const [dialogOpen, setDialogOpen] = React.useState(false);

  const [editingOrder, setEditingOrder] = React.useState<Order | undefined>(undefined);

  React.useEffect(() => {
    setLoading(true);
    setError(null);
    api.get('/api/orders')
      .then(res => {
        // Map backend data to table format
        // If API returns paged result: { items: [...], ... }
        const items = res.data.items || res.data;
        // Flatten items for table: show first product/customer for MVP
        const mapped = items.map((order: any) => ({
          id: order.id,
          customer: order.customerName || 'N/A',
          product: order.items && order.items[0] ? `Product #${order.items[0].productId}` : 'N/A',
          quantity: order.items && order.items[0] ? order.items[0].quantity : 0,
        }));
        setOrders(mapped);
      })
      .catch(_err => {
        setError('Failed to load orders');
      })
      .finally(() => setLoading(false));
  }, []);

  const handleAdd = () => {
    setEditingOrder({ customer: '', product: '', quantity: 1 });
    setDialogOpen(true);
  };

  const handleEdit = (order: any) => {
    // Ensure all fields are present
    setEditingOrder({
      id: order.id,
      customer: order.customer ?? '',
      product: order.product ?? '',
      quantity: order.quantity ?? 1,
    });
    setDialogOpen(true);
  };

  const handleClose = () => {
    setDialogOpen(false);
  };

  const handleSave = (order: Order) => {
    setOrders((prev) => {
      if (order.id) {
        return prev.map((o) => (o.id === order.id ? order : o));
      } else {
        return [...prev, { ...order, id: prev.length ? Math.max(...prev.map((o) => o.id)) + 1 : 1 }];
      }
    });
    setDialogOpen(false);
  };

  const [detailsOpen, setDetailsOpen] = React.useState(false);
  const [viewingOrder, setViewingOrder] = React.useState<Order | undefined>(undefined);

  const handleView = (order: Order) => {
    setViewingOrder(order);
    setDetailsOpen(true);
  };
  const handleDetailsClose = () => {
    setDetailsOpen(false);
    setViewingOrder(undefined);
  };

  const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', width: 70 },
    { field: 'customer', headerName: 'Customer', flex: 1 },
    { field: 'product', headerName: 'Product', flex: 1 },
    { field: 'quantity', headerName: 'Quantity', type: 'number', width: 120 },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 170,
      renderCell: (params) => (
        <>
          <Button size="small" onClick={() => handleView(params.row)} style={{ marginRight: 8 }} variant="outlined">View</Button>
          <Button size="small" onClick={() => handleEdit(params.row)} variant="outlined">Edit</Button>
        </>
      ),
      sortable: false,
      filterable: false,
      disableColumnMenu: true,
    },
  ];

  return (
    <Stack spacing={2}>
      <Stack direction="row" justifyContent="space-between" alignItems="center">
        <h2 style={{ margin: 0 }}>Orders</h2>
        <Button variant="contained" onClick={handleAdd}>New Order</Button>
      </Stack>
      <div style={{ height: 600, width: '100%', background: '#fff', borderRadius: 8, position: 'relative' }}>
        {loading ? (
          <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%' }}>
            <span>Loading...</span>
          </div>
        ) : error ? (
          <div style={{ color: 'red', display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%' }}>{error}</div>
        ) : (
          <DataGrid
            rows={orders}
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
      <OrderDialog
        open={dialogOpen}
        onClose={handleClose}
        order={editingOrder}
        onSave={handleSave}
      />
      <OrderDetailsDialog
        open={detailsOpen}
        onClose={handleDetailsClose}
        order={viewingOrder}
      />
    </Stack>
  );
}

