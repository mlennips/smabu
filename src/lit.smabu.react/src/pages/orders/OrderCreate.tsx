import { useState, useEffect } from 'react';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import { deepValueChange } from '../../utils/deepValueChange';
import createId from '../../utils/createId';
import { useNavigate } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import DefaultContentContainer from '../../components/contentBlocks/DefaultContentBlock';
import { createOrder } from '../../services/order.service';
import { CreateOrderCommand, CustomerDTO, OrderId } from '../../types/domain';
import { getCustomers } from '../../services/customer.service';
import { CreateActions } from '../../components/contentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import SelectField from '../../components/controls/SelectField';

const OrderCreate = () => {
    const [data, setData] = useState<CreateOrderCommand>({
        orderId: createId<OrderId>(),
        name: 'Neuer Auftrag',
        orderDate: new Date(),
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const [customers, setCustomers] = useState<CustomerDTO[]>();
    const navigate = useNavigate();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: getCustomers,
            onLoading: setLoading,
            onSuccess: (response) => {
                setCustomers(response);
            },
            onError: (error) => setError(error)
        });
    }, []);

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => createOrder(data),
            onLoading: setLoading,
            onSuccess: (_response) => {
                toast("Auftrag erfolgreich erstellt", "success");
                navigate(`/orders/${data?.orderId?.value}`);
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer subtitle={data?.name} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={1}>
                            <Grid size={{ xs: 12, sm: 6 }}>
                                <SelectField
                                    label='Kunde'
                                    name='customerId'
                                    required
                                    value={data?.customerId?.value}
                                    onChange={handleChange}
                                    items={customers ?? []}
                                    onGetValue={(item) => item.id.value}
                                    onGetLabel={(item) => item.name} />
                            </Grid>
                            <Grid size={{ xs: 12, sm: 6 }}><TextField type='date' fullWidth label="Datum" name="orderDate" value={data?.orderDate?.toISOString().split('T')[0]} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 12, sm: 12 }}><TextField fullWidth label="Name" name="name" value={data?.name} onChange={handleChange} required /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >
                <CreateActions formId="form" disabled={loading} />
            </Stack>
        </form>
    );
};

export default OrderCreate;
