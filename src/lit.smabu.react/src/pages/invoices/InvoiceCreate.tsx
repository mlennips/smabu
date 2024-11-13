import { useState, useEffect } from 'react';
import { CreateInvoiceCommand, Currency, CustomerDTO, InvoiceDTO, InvoiceId } from '../../types/domain';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import { deepValueChange } from '../../utils/deepValueChange';
import createId from '../../utils/createId';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import DefaultContentContainer from '../../components/contentBlocks/DefaultContentBlock';
import { getCustomers } from '../../services/customer.service';
import { createInvoice, getInvoices } from '../../services/invoice.service';
import { formatDate, formatToDateOnly } from '../../utils/formatDate';
import { CreateActions } from '../../components/contentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import SelectField from '../../components/controls/SelectField';

const defaultCurrency: Currency = {
    isoCode: 'EUR',
    name: 'Euro',
    sign: '€'
};

const InvoiceCreate = () => {
    const [searchParams] = useSearchParams();
    const templateId = searchParams.get('templateId');
    const [data, setData] = useState<CreateInvoiceCommand>({
        invoiceId: createId<InvoiceId>(),
        fiscalYear: new Date().getFullYear(),
        currency: defaultCurrency,
        performancePeriod: { from: formatToDateOnly(new Date().toISOString()) },
        customerId: { value: '' },
        templateId: templateId ? { value: templateId } : undefined
    });
    const [loading, setLoading] = useState(true);
    const [customers, setCustomers] = useState<CustomerDTO[]>();
    const [invoices, setInvoices] = useState<InvoiceDTO[]>();
    const [error, setError] = useState(undefined);
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

        handleAsyncTask({
            task: getInvoices,
            onLoading: setLoading,
            onSuccess: (response) => {
                if (templateId) {
                    const template = response.find(x => x.id?.value === templateId);
                    if (template) {
                        data.customerId = template.customer?.id ?? { value: '' };
                    }
                }
                setInvoices(response);
            },
            onError: (error) => setError(error)
        });
    }, []);

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        if (name === 'templateId') {
            data.customerId = invoices?.find(x => x.id?.value === value?.value)?.customer?.id ?? { value: '' };
            value = data.customerId?.value == '' ? undefined : value;
        }
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => createInvoice(data),
            onLoading: setLoading,
            onSuccess: (_response) => {
                toast("Rechnung erfolgreich erstellt", "success");
                navigate(`/invoices/${data.invoiceId?.value}`);
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={1}>
                            <Grid size={{ xs: 6 }}><TextField type='number' fullWidth label="Geschäftsjahr" name="fiscalYear" value={data?.fiscalYear} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 6 }}><TextField fullWidth label="Währung" name="currency" value={data?.currency?.isoCode} onChange={handleChange} required disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 6 }}>
                                <SelectField
                                    label='Vorlage'
                                    name='templateId'
                                    value={data?.templateId?.value}
                                    onChange={handleChange}
                                    items={invoices ?? []}
                                    onGetValue={(item) => item.id.value}
                                    onGetLabel={(item) => `${item.displayName} ${formatDate(item.releasedAt)} (${item.amount?.toFixed(2)} ${item.currency?.isoCode})`} />
                            </Grid>
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
                        </Grid>
                    </Paper>
                </DefaultContentContainer >
                <CreateActions formId="form" />
            </Stack>
        </form>
    );
};

export default InvoiceCreate;
