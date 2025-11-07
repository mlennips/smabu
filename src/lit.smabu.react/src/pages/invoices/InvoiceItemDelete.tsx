import { useState, useEffect } from 'react';
import { InvoiceDTO, InvoiceItemDTO } from '../../types/domain';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { deleteInvoiceItem, getInvoice } from '../../services/invoice.service';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const InvoiceDelete = () => {
    const [invoice, setInvoice] = useState<InvoiceDTO>();
    const [data, setData] = useState<InvoiceItemDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () => getInvoice(params.invoiceId!, true),
            onLoading: (loading) => setLoading(loading),
            onSuccess: (response) => {
                setInvoice(response);
                setData(response.items?.find((item: InvoiceItemDTO) => item.id!.value === params.invoiceItemId));
            },
            onError: setError
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();

        handleAsyncTask({
            task: () => deleteInvoiceItem(params.invoiceId!, params.invoiceItemId!),
            onLoading: (loading) => setLoading(loading),
            onSuccess: () => {
                toast("Position erfolgreich gelöscht", "success");
                navigate(`/invoices/${params.invoiceId}`);
            },
            onError: (error) => setError(error)});
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DetailPageContainer title={invoice?.displayName} subtitle={`Pos: ${data?.displayName}`} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        Soll die Rechnungsposition "{data?.position}" wirklich gelöscht werden?
                        <br />
                        Details: {data?.details}
                    </Paper>
                </DetailPageContainer>
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default InvoiceDelete;
