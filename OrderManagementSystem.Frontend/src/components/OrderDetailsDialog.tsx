import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import type { Order } from '../pages/Orders';

export interface OrderDetailsDialogProps {
  open: boolean;
  onClose: () => void;
  order?: Order;
}

export default function OrderDetailsDialog({ open, onClose, order }: OrderDetailsDialogProps) {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
      <DialogTitle>Order Details</DialogTitle>
      <DialogContent>
        {order ? (
          <>
            <Typography variant="body1">
              <strong>ID:</strong> {order.id ?? 'New'}
            </Typography>
            <Typography variant="body1">
              <strong>Customer:</strong> {order.customer}
            </Typography>
            <Typography variant="body1">
              <strong>Product:</strong> {order.product}
            </Typography>
            <Typography variant="body1">
              <strong>Quantity:</strong> {order.quantity}
            </Typography>
          </>
        ) : (
          <Typography variant="body2" color="textSecondary">
            No order selected.
          </Typography>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Close</Button>
      </DialogActions>
    </Dialog>
  );
}
