import { useState, useEffect } from 'react';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { OrderDTO } from '../../types/domain';
import { deleteOrder, getOrder } from '../../services/order.service';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const OrderDelete = () => {
    const [data, setData] = useState<OrderDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () => getOrder(params.orderId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => deleteOrder(params.orderId!),
            onLoading: setLoading,
            onSuccess: (_response) => {
                toast("Auftrag erfolgreich gelöscht", "success");
                navigate('/orders');
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DetailPageContainer subtitle={data?.number?.value?.toString()} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        Soll der Auftrag "{data?.number?.value}" wirklich gelöscht werden?
                    </Paper>
                </DetailPageContainer >
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default OrderDelete;