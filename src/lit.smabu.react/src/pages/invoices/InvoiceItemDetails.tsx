import { useState, useEffect } from 'react';
import { InvoiceDTO, InvoiceItemDTO } from '../../types/domain';
import { useParams } from 'react-router-dom';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNotification } from '../../contexts/notificationContext';
import { getInvoice, updateInvoiceItem } from '../../services/invoice.service';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { UnitSelectField } from '../../components/controls/SelectField';
import SelectCatalogItemComponent from '../catalogs/SelectCatalogItemComponent';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const InvoiceItemDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [invoice, setInvoice] = useState<InvoiceDTO>();
    const [data, setData] = useState<InvoiceItemDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);

    useEffect(() => {
        loadData();
    }, [params.invoiceId, params.invoiceItemId]);

    const loadData = () => handleAsyncTask({
        task: () => getInvoice(params.invoiceId!, true),
        onLoading: setLoading,
        onSuccess: (response) => {
            setInvoice(response);
            setData(response.items?.find((item: InvoiceItemDTO) => item.id!.value === params.invoiceItemId));
        },
        onError: setError
    });
    
    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();

        handleAsyncTask({
            task: () => updateInvoiceItem(params.invoiceId!, params.invoiceItemId!, {
                invoiceItemId: data?.id!,
                invoiceId: data?.invoiceId!,
                quantity: data?.quantity!,
                unitPrice: data?.unitPrice!,
                details: data?.details!,
                catalogItemId: data?.catalogItemId!
            }),
            onLoading: (loading) => setLoading(loading),
            onSuccess: () => {
                toast("Rechnungsposition erfolgreich gespeichert", "success");
                loadData();
            },
            onError: (error) => setError(error),
        });
    };

    const toolbarItems: ToolbarItem[] = [];

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer title={invoice?.displayName} subtitle={"#" + data?.displayName} loading={loading} error={error} toolbarItems={toolbarItems} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 12, sm: 2, md: 2 }}><TextField fullWidth label="Position" name="position" value={data?.position} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 5, md: 4 }}><TextField fullWidth label="Rechnung" name="invoice" value={invoice?.displayName} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 5, md: 6 }}><TextField fullWidth label="Kunde" name="customer" value={invoice?.customer?.name} disabled /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer>

                <DefaultContentContainer title="Details" loading={loading}>
                    <SelectCatalogItemComponent
                        customerId={invoice?.customer?.id!}
                        getCatalogItemId={() => data?.catalogItemId}
                        setCatalogItemId={(value) => setData(deepValueChange(data, "catalogItemId", value))}
                        setDetails={(value) => setData(deepValueChange(data, 'details', value))}
                        setPrice={(value) => setData(deepValueChange(data, 'unitPrice', value))}
                        setUnit={(value) => setData(deepValueChange(data, 'quantity.unit', value))} />
                    <TextField multiline variant='standard' minRows={5} maxRows={10} fullWidth
                        name="details" value={data?.details} onChange={handleChange} />
                </DefaultContentContainer>

                <DefaultContentContainer title={invoice?.displayName} subtitle={"#" + data?.displayName} loading={loading} toolbarItems={toolbarItems} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Anzahl" name="quantity.value" value={data?.quantity?.value} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}>
                                <UnitSelectField name="quantity.unit" value={data?.quantity?.unit?.value} required
                                    onChange={handleChange} label={'Einheit'} />
                            </Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Einzelpreis" name="unitPrice" value={data?.unitPrice} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Gesamt" name="totalPrice" value={data?.totalPrice} disabled /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >

                <DetailsActions formId="form" deleteUrl={`/invoices/${params.invoiceId}/items/${data?.id?.value}/delete`} disabled={loading} />
            </Stack>
        </form>
    );
};

export default InvoiceItemDetails;