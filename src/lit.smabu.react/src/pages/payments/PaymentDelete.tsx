import { useState, useEffect } from 'react';
import { PaymentDTO } from '../../types/domain';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { deletePayment, getPayment } from '../../services/payments.services';

const PaymentDelete = () => {
    const [data, setData] = useState<PaymentDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () => getPayment(params.paymentId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => deletePayment(params.paymentId!),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Zahlung erfolgreich gelöscht", "success");
                navigate('/payments');
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DetailPageContainer subtitle={data?.displayName} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        Soll die Zahlung "{data?.number?.displayName}" wirklich gelöscht werden?
                    </Paper>
                </DetailPageContainer >
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default PaymentDelete;
