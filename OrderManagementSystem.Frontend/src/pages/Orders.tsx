import * as React from 'react';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import OrderDialog from '../components/OrderDialog';

const initialOrders = [
  { id: 1, customer: 'Alice', product: 'Widget', quantity: 3 },
  { id: 2, customer: 'Bob', product: 'Gadget', quantity: 1 },
  { id: 3, customer: 'Charlie', product: 'Thingamajig', quantity: 2 },
];

export default function Orders() {
  const [orders, setOrders] = React.useState(initialOrders);
  const [dialogOpen, setDialogOpen] = React.useState(false);
  const [editingOrder, setEditingOrder] = React.useState<{ id?: number; customer: string; product: string; quantity: number; } | undefined>(undefined);

  const handleAdd = () => {
    setEditingOrder(undefined);
    setDialogOpen(true);
  };

  const handleEdit = (order: any) => {
    setEditingOrder(order);
    setDialogOpen(true);
  };

  const handleClose = () => {
    setDialogOpen(false);
  };

  const handleSave = (order: any) => {
    setOrders((prev) => {
      if (order.id) {
        return prev.map((o) => (o.id === order.id ? order : o));
      } else {
        return [...prev, { ...order, id: prev.length ? Math.max(...prev.map((o) => o.id)) + 1 : 1 }];
      }
    });
    setDialogOpen(false);
  };

  const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', width: 70 },
    { field: 'customer', headerName: 'Customer', flex: 1 },
    { field: 'product', headerName: 'Product', flex: 1 },
    { field: 'quantity', headerName: 'Quantity', type: 'number', width: 120 },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 120,
      renderCell: (params) => (
        <Button size="small" onClick={() => handleEdit(params.row)} variant="outlined">Edit</Button>
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
      <div style={{ height: 350, width: '100%', background: '#fff', borderRadius: 8 }}>
        <DataGrid
          rows={orders}
          columns={columns}
          pageSizeOptions={[5]}
          initialState={{
            pagination: {
              paginationModel: { pageSize: 5, page: 0 },
            },
          }}
          disableRowSelectionOnClick
        />
      </div>
      <OrderDialog open={dialogOpen} onClose={handleClose} order={editingOrder} onSave={handleSave} />
    </Stack>
  );
}

