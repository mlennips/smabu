import { useState, useEffect } from 'react';
import { InvoiceDTO, InvoiceItemDTO } from '../../types/domain';
import { Button, ButtonGroup, Grid2 as Grid, Paper } from '@mui/material';
import DetailPageContainer from '../../containers/DefaultContentContainer';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { deleteInvoiceItem, getInvoice } from '../../services/invoice.service';

const InvoiceDelete = () => {
    const [invoice, setInvoice] = useState<InvoiceDTO>();
    const [data, setData] = useState<InvoiceItemDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        getInvoice(params.invoiceId!, true)
            .then(response => {
                setInvoice(response.data);
                setData(response.data.items?.find((item: InvoiceItemDTO) => item.id!.value === params.id));
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        deleteInvoiceItem(params.invoiceId!, params.id!)
            .then((_response) => {
                setLoading(false);
                toast("Position erfolgreich gelöscht", "success");
                navigate(`/invoices/${params.invoiceId}`);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Grid container spacing={2}>
                <Grid size={{ xs: 12 }}>
                    <DetailPageContainer title={invoice?.displayName} subtitle={`Pos: ${data?.displayName}`} loading={loading} error={error} >
                        <Paper sx={{ p: 2 }}>
                            Soll die Rechnungsposition "{data?.position}" wirklich gelöscht werden?
                            <br />
                            Details: {data?.details}
                        </Paper>
                    </DetailPageContainer >
                </Grid>
                <Grid size={{ xs: 12 }}>
                    <ButtonGroup disabled={loading}>
                        <Button type="submit" variant="contained" color="warning">
                            Löschen
                        </Button>
                    </ButtonGroup>
                </Grid>
            </Grid>
        </form>
    );
};

export default InvoiceDelete;
