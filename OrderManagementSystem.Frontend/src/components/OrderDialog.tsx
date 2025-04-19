import * as React from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';

export interface OrderDialogProps {
  open: boolean;
  onClose: () => void;
  order?: { id?: number; customer: string; product: string; quantity: number };
  onSave: (order: { id?: number; customer: string; product: string; quantity: number }) => void;
}

export default function OrderDialog({ open, onClose, order, onSave }: OrderDialogProps) {
  const [form, setForm] = React.useState(order || { customer: '', product: '', quantity: 1 });

  React.useEffect(() => {
    setForm(order || { customer: '', product: '', quantity: 1 });
  }, [order]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSave = () => {
    onSave({ ...form, quantity: Number(form.quantity) });
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
        />
        <TextField
          margin="normal"
          label="Product"
          name="product"
          value={form.product}
          onChange={handleChange}
          fullWidth
        />
        <TextField
          margin="normal"
          label="Quantity"
          name="quantity"
          type="number"
          value={form.quantity}
          onChange={handleChange}
          fullWidth
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSave} variant="contained">Save</Button>
      </DialogActions>
    </Dialog>
  );
}
