import { useState, useEffect } from 'react';
import { InvoiceDTO } from '../../types/domain';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { deleteInvoice, getInvoice } from '../../services/invoice.service';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
const InvoiceDelete = () => {
    const [data, setData] = useState<InvoiceDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () =>getInvoice(params.invoiceId!, true),
            onLoading: (loading) => setLoading(loading),
            onSuccess: setData,
            onError: (error) => setError(error)
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();

        handleAsyncTask({
            task: () => deleteInvoice(params.invoiceId!),
            onLoading: (loading) => setLoading(loading),
            onSuccess: () => {
                toast("Rechnung erfolgreich gelöscht", "success");
                navigate('/invoices');
            },
            onError: (error) => setError(error)
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DetailPageContainer subtitle={data?.number?.value?.toString()} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        Soll die Rechnung "{data?.number?.value}" wirklich gelöscht werden?
                    </Paper>
                </DetailPageContainer >
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default InvoiceDelete;
