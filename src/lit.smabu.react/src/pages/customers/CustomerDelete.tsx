import { useState, useEffect } from 'react';
import { CustomerDTO } from '../../types/domain';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { deleteCustomer, getCustomer } from '../../services/customer.service';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const CustomerDelete = () => {
    const [data, setData] = useState<CustomerDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () => getCustomer(params.customerId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => deleteCustomer(params.customerId!),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Kunde erfolgreich gelöscht", "success");
                navigate('/customers');
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DetailPageContainer subtitle={data?.name} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        Soll der Kunde "{data?.name}" wirklich gelöscht werden?
                    </Paper>
                </DetailPageContainer >
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default CustomerDelete;
