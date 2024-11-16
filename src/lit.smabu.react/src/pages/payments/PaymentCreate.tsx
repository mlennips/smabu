import { useState, useEffect } from 'react';
import { CreatePaymentCommand, CustomerDTO, InvoiceDTO } from '../../types/domain';
import { Box, Grid2 as Grid, Paper, Stack, Tab, TextField } from '@mui/material';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNavigate } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import DefaultContentContainer from '../../components/contentBlocks/DefaultContentBlock';
import { CreateActions } from '../../components/contentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { createPayment } from '../../services/payments.services';
import createId from '../../utils/createId';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import React from 'react';
import SelectField from '../../components/controls/SelectField';
import { getCustomers } from '../../services/customer.service';
import { getInvoicesByCustomerId } from '../../services/invoice.service';
import { formatForTextField } from '../../utils/formatDate';

const PaymentCreate = () => {
    const [tabValue, setTabValue] = React.useState('Incoming');
    const [data, setData] = useState<CreatePaymentCommand>({
        paymentId: createId(),
        direction: { value: tabValue },
        details: "",
        payer: "",
        payee: "",
        referenceNr: "",
        referenceDate: undefined,
        amountDue: 0,
        dueDate: undefined,
        customerId: undefined,
        invoiceId: undefined,
        markAsPaid: false
    });
    const [customers, setCustomers] = useState<CustomerDTO[]>();
    const [invoices, setInvoices] = useState<InvoiceDTO[]>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const { toast } = useNotification();

    useEffect(() => {
        if (tabValue === 'Incoming') {
            if (customers === undefined) {
                handleAsyncTask({
                    task: getCustomers,
                    onLoading: setLoading,
                    onSuccess: (response) => {
                        setCustomers(response);
                    },
                    onError: (error) => setError(error)
                });
            }
        }
    }, []);

    useEffect(() => {
        if (tabValue === 'Incoming') {
            if (data.customerId && data.customerId.value) {
                handleAsyncTask({
                    task: () => getInvoicesByCustomerId(data.customerId?.value!),
                    onLoading: setLoading,
                    onSuccess: (response) => {
                        setInvoices(response);
                    },
                    onError: (error) => setError(error)
                });
            }
        }
    }, [data.customerId]);

    const handleChangeTab = (_event: React.SyntheticEvent, newValue: string) => {
        setData(deepValueChange(data, 'direction', { value: newValue }));
        setTabValue(newValue);
    };

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        if (name === 'markAsPaid') {
            value = (value === 'true');
        }
        if (name === 'invoiceId' && value !== undefined) {
            const invoice = invoices?.find(i => i.id?.value === value.value);
            data.amountDue = invoice?.amount;
            data.referenceDate = invoice?.invoiceDate ? new Date(invoice.invoiceDate) : undefined;
            data.referenceNr = invoice?.number?.displayName!;
            data.payer = invoice?.customer?.name;
        }
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => createPayment(data),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Zahlung erfolgreich erstellt", "success");
                navigate(`/payments/${data.paymentId!.value}`);
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer subtitle={""} loading={loading} error={error} >
                    <TabContext value={tabValue}>
                        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                            <TabList onChange={handleChangeTab}>
                                <Tab label="Eingehend" value="Incoming" />
                                <Tab label="Ausgehend" value="Outgoing" />
                            </TabList>
                        </Box>
                        <Paper>
                            {renderIncomingTab()}
                            {renderOutgoingTab()}
                        </Paper>
                    </TabContext>
                </DefaultContentContainer >
                <CreateActions formId="form" disabled={loading} />
            </Stack>
        </form>
    );

    function renderIncomingTab() {
        return <TabPanel value="Incoming">
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
                <Grid size={{ xs: 12, sm: 6 }}>
                    <SelectField
                        label='Rechnung'
                        name='invoiceId'
                        required
                        value={data?.invoiceId?.value}
                        onChange={handleChange}
                        items={invoices?.sort((a, b) => new Date(b.createdAt!).getTime() - new Date(a.createdAt!).getTime()) ?? []}
                        onGetValue={(item) => item.id.value}
                        onGetLabel={(item) => item.displayName} />
                </Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Zahlender" name="payer"
                    value={data.payer} onChange={handleChange} required /></Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Referenz Nr." name="referenceNr"
                    value={data.referenceNr} onChange={handleChange} required /></Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Referenz Datum" name="referenceDate"
                    type='date' value={formatForTextField(data.referenceDate, 'date')} onChange={handleChange} required /></Grid>
                <Grid size={{ xs: 12, sm: 6 }}><TextField fullWidth label="Betrag" name="amountDue" type="number"
                    value={data.amountDue} onChange={handleChange} /></Grid>
                <Grid size={{ xs: 12, sm: 6 }}><TextField type='date' fullWidth label="Fällig am" name="dueDate"
                    value={data.dueDate} onChange={handleChange} /></Grid>
                <Grid size={{ xs: 12 }}><TextField fullWidth label="Details" name="details"
                    value={data.details} onChange={handleChange} /></Grid>
            </Grid>
        </TabPanel>
    }

    function renderOutgoingTab() {
        return <TabPanel value="Outgoing">
            <Grid container spacing={1}>
                <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Zahlungsempfänger" name="payee"
                    value={data.payee} onChange={handleChange} required /></Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Referenz Nr." name="referenceNr"
                    value={data.referenceNr} onChange={handleChange} required /></Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Referenz Datum" name="referenceDate"
                    type='date' value={formatForTextField(data.referenceDate, 'date')} onChange={handleChange} required /></Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Betrag" name="amountDue" type="number"
                    value={data.amountDue} onChange={handleChange} /></Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField type='date' fullWidth label="Fällig am" name="dueDate"
                    value={data.dueDate} onChange={handleChange} /></Grid>
                <Grid size={{ xs: 12, sm: 4 }}><TextField type='checkbox' fullWidth label="Abschließen?" name="markAsPaid"
                    value={data.markAsPaid ?? false ? 'false' : 'true'} onChange={handleChange} /></Grid>
                <Grid size={{ xs: 12 }}><TextField fullWidth label="Details" name="details"
                    value={data.details} onChange={handleChange} /></Grid>
            </Grid>
        </TabPanel>
    }
};

export default PaymentCreate;
