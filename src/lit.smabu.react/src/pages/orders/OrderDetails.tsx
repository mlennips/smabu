import { useState, useEffect } from 'react';
import { OrderDTO } from '../../types/domain';
import { useParams } from 'react-router-dom';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNotification } from '../../contexts/notificationContext';
import { getOrder, updateOrder } from '../../services/order.service';
import OrderReferencesComponent from './OrderReferencesComponent';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const OrderDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [data, setData] = useState<OrderDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);

    useEffect(() => {
        handleAsyncTask({
            task: () => getOrder(params.orderId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, [params.orderId]);

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => updateOrder(params.orderId!, {
                orderId: data?.id!,
                name: data?.name!,
                orderDate: data?.orderDate!,
                deadline: data?.deadline,
                description: data?.description!,
                bunchKey: data?.bunchKey
            }),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Auftrag erfolgreich gespeichert", "success");
            },
            onError: setError
        });
    };

    return <Grid container spacing={2}>
        {data && renderDetails(handleSubmit, data, loading, error, handleChange)}
        {renderReferences(params.orderId!)}
    </Grid>;
};


function renderDetails(handleSubmit: (event: React.FormEvent) => void, data: OrderDTO | undefined, loading: boolean, error: any, handleChange: (e: any) => void) {
    const toolbarItems: ToolbarItem[] = [];
    return <form id="form" onSubmit={handleSubmit}>
        <Stack spacing={2}>
            <DefaultContentContainer subtitle={data?.displayName} loading={loading} error={error} toolbarItems={toolbarItems}>
                <Paper sx={{ p: 2 }}>
                    <Grid container spacing={2}>
                        <Grid size={{ xs: 12, sm: 6, md: 2 }}><TextField fullWidth label="#" name="number" value={data?.number?.value} disabled /></Grid>
                        <Grid size={{ xs: 12, sm: 12, md: 2 }}><TextField type='date' fullWidth label="Datum" name="orderDate" value={data?.orderDate} onChange={handleChange} required /></Grid>
                        <Grid size={{ xs: 12, sm: 12, md: 4 }}><TextField fullWidth label="Kunde" name="customer" value={data?.customer?.name} /></Grid>
                        <Grid size={{ xs: 12, sm: 12, md: 4 }}><TextField fullWidth label="Bündel-Id" name="bunchKey" value={data?.bunchKey} onChange={handleChange} /></Grid>
                        <Grid size={{ xs: 12, sm: 12, md: 10 }}><TextField fullWidth label="Bezeichung" name="name" value={data?.name} onChange={handleChange} required /></Grid>
                        <Grid size={{ xs: 12, sm: 12, md: 2 }}><TextField type='datetime-local' fullWidth label="Deadline" name="deadline" value={data?.deadline ?? undefined} onChange={handleChange} /></Grid>
                        <Grid size={{ xs: 12, sm: 12, md: 12 }}><TextField multiline fullWidth label="Beschreibung" name="description" value={data?.description} onChange={handleChange} minRows={4} /></Grid>
                    </Grid>
                </Paper>
            </DefaultContentContainer>
            <DetailsActions formId="form" deleteUrl={`/orders/${data?.id?.value}/delete`} disabled={loading} />
        </Stack>
    </form>;
}

function renderReferences(orderId: string) {
    const [toolbar, setToolbar] = useState<ToolbarItem[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);

    return <Grid size={{ xs: 12 }}>
        <DefaultContentContainer title='Verknüpfungen' toolbarItems={toolbar} loading={loading} error={error}>
            {OrderReferencesComponent({ orderId: orderId, setError: setError, setToolbar: setToolbar, setLoading: setLoading })}
        </DefaultContentContainer>
    </Grid>
}

export default OrderDetails;