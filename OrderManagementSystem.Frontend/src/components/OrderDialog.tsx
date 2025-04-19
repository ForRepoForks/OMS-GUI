import * as React from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';

import type { Order } from '../pages/Orders';

export interface OrderDialogProps {
  open: boolean;
  onClose: () => void;
  order?: Order;
  onSave: (order: Order) => void;
}

export default function OrderDialog({ open, onClose, order, onSave }: OrderDialogProps) {
  const [form, setForm] = React.useState(order || { customer: '', product: '', quantity: 1 });
  const [touched, setTouched] = React.useState<{ [key: string]: boolean }>({});

  React.useEffect(() => {
    setForm(order || { customer: '', product: '', quantity: 1 });
    setTouched({});
  }, [order]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({
      ...form,
      [e.target.name]:
        e.target.name === 'quantity' ? e.target.value.replace(/[^\d]/g, '') : e.target.value,
    });
    setTouched({ ...touched, [e.target.name]: true });
  };

  const isCustomerValid = form.customer.trim().length > 0;
  const isProductValid = form.product.trim().length > 0;
  const isQuantityValid = String(form.quantity).trim().length > 0 && Number(form.quantity) > 0;
  const isFormValid = isCustomerValid && isProductValid && isQuantityValid;

  const handleSave = () => {
    if (isFormValid) {
      onSave({ ...form, quantity: Number(form.quantity) });
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
      <DialogTitle>{order ? 'Edit Order' : 'New Order'}</DialogTitle>
      <DialogContent>
        <TextField
          margin="normal"
          label="Customer"
          name="customer"
          value={form.customer}
          onChange={handleChange}
          fullWidth
          required
          error={touched.customer && !isCustomerValid}
          helperText={touched.customer && !isCustomerValid ? 'Customer is required' : ''}
        />
        <TextField
          margin="normal"
          label="Product"
          name="product"
          value={form.product}
          onChange={handleChange}
          fullWidth
          required
          error={touched.product && !isProductValid}
          helperText={touched.product && !isProductValid ? 'Product is required' : ''}
        />
        <TextField
          margin="normal"
          label="Quantity"
          name="quantity"
          type="number"
          value={form.quantity}
          onChange={handleChange}
          fullWidth
          required
          error={touched.quantity && !isQuantityValid}
          helperText={
            touched.quantity && !isQuantityValid ? 'Quantity must be a positive number' : ''
          }
          inputProps={{ min: 1 }}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSave} variant="contained" disabled={!isFormValid}>
          {order ? 'Update' : 'Create'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
