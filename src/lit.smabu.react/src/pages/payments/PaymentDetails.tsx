import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNotification } from '../../contexts/notificationContext';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { completePayment, getPayment, updatePayment } from '../../services/payments.services';
import { PaymentDTO } from '../../types/domain';
import { formatForTextField } from '../../utils/formatDate';
import { Check, Redo } from '@mui/icons-material';
import { PaymentMethodSelectField } from '../../components/controls/SelectField';

const PaymentDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [data, setData] = useState<PaymentDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [];
    if (data?.status?.value === 'Pending') {
        toolbarItems.push({
            text: "Abschließen",
            icon: <Check />,
            action: () => complete()
        });
    } else if (data?.status?.value === 'Paid') {
        toolbarItems.push({
            text: "Wieder öffnen",
            icon: <Redo />,
            action: () => { 
                 setData(deepValueChange(data, 'status', { value: 'Pending' }));
            }
        });
    }

    useEffect(() => {
        loadData();
    }, [params.paymentId]);

    const loadData = () => {
        handleAsyncTask({
            task: () => getPayment(params.paymentId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => updatePayment(params.paymentId!, {
                paymentId: data?.id,
                accountingDate: data?.accountingDate,
                status: data?.status,
                amountDue: data?.amountDue,
                dueDate: data?.dueDate,
                referenceDate: data?.referenceDate,
                referenceNr: data?.referenceNr,
                details: data?.details,
                payer: data?.payer,
                payee: data?.payee,
                paymentMethod: data?.paymentMethod,
            }),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Zahlung erfolgreich gespeichert", "success");
            },
            onError: setError
        });
    };

    const complete = () => {
        handleAsyncTask({
            task: () => completePayment(params.paymentId!, {
                paymentId: data?.id,
                amount: (data?.amountPaid ?? 0 >= 0 ? data?.amountPaid : data?.amountDue!),
                paidAt: data?.paidAt ?? new Date()
            }),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Zahlung erfolgreich abgeschlossen", "success");
                loadData();
            },
            onError: setError
        });
    };

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <Grid container spacing={2}>
                    <Grid size={{ xs: 12 }}>
                        <DefaultContentContainer subtitle={data?.displayName} loading={loading} error={error} toolbarItems={toolbarItems} >
                            <Paper sx={{ p: 2 }}>
                                <Grid container spacing={1}>
                                    <Grid size={{ xs: 12, sm: 3 }}><TextField fullWidth label="#" name="number"
                                        value={data?.number?.value} onChange={handleChange} required disabled /></Grid>
                                    <Grid size={{ xs: 12, sm: 3 }}><TextField fullWidth label="Status" name="status"
                                        value={data?.status?.value} onChange={handleChange} required disabled /></Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}><TextField fullWidth label="Buchungsdatum" name="accountingDate"
                                        type='date' value={formatForTextField(data?.accountingDate, 'date')} onChange={handleChange} required /></Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}><TextField fullWidth label="Zahler" name="payer"
                                        value={data?.payer} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}><TextField fullWidth label="Zahlungsempfänger" name="payee"
                                        value={data?.payee} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}><TextField fullWidth label="Referenz Nr." name="referenceNr"
                                        value={data?.referenceNr} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}><TextField fullWidth label="Referenz Datum" name="referenceDate"
                                        type='date' value={formatForTextField(data?.referenceDate, 'date')} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}><TextField fullWidth label="Betrag" name="amountDue" type="number"
                                        value={data?.amountDue} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}><TextField type='date' fullWidth label="Fällig am" name="dueDate"
                                        value={data?.dueDate} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Beglichener Betrag" name="amountPaid"
                                        value={data?.amountPaid} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 4 }}><TextField fullWidth label="Beglichen am" name="paidAt"
                                        type='date' value={formatForTextField(data?.paidAt, 'date')} onChange={handleChange} /></Grid>                                    
                                    <Grid size={{ xs: 12, sm: 4 }}><PaymentMethodSelectField label='Zahlungsweise' name="paymentMethod" value={data?.paymentMethod?.value}
                                        onChange={handleChange} required /></Grid>
                                    <Grid size={{ xs: 12 }}><TextField fullWidth label="Details" name="details"
                                        value={data?.details} onChange={handleChange} /></Grid>
                                </Grid>

                            </Paper>
                        </DefaultContentContainer >
                    </Grid>
                </Grid>
                <DetailsActions formId="form" deleteUrl={`/payments/${data?.id?.value}/delete`} disabled={loading} />
            </Stack>
        </form>
    );
};

export default PaymentDetails;


