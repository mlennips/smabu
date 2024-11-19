
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { AnnualFinancialStatementDTO } from '../../types/domain/annual-financial-statement-dto';
import { getAnnualFinancialStatement, importAnnualFinancialStatementTransactions, updateAnnualFinancialStatement } from '../../services/financials.service';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { useNotification } from '../../contexts/notificationContext';
import { deepValueChange } from '../../utils/deepValueChange';
import { Grid2 as Grid, IconButton, InputAdornment, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField } from '@mui/material';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { AppError } from '../../utils/errorConverter';
import { AddCircle, ImportExport, Lock, Remove } from '@mui/icons-material';
import { formatForTextField } from '../../utils/formatDate';
import { FinancialCategorySelectField } from '../../components/controls/SelectField';
import { Currency, DatePeriod, FinancialTransaction } from '../../types/domain';

const AnnualFinancialStatementDetails: React.FC = () => {
    const { annualStatementId: annualFinancialStatementId } = useParams<{ annualStatementId: string }>();
    const { toast } = useNotification();
    const [data, setData] = useState<AnnualFinancialStatementDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<AppError | undefined>(undefined);
    const toolbarItems: ToolbarItem[] = [];
    const toolbarItemsIncome: ToolbarItem[] = [
        {
            text: "Importieren",
            icon: <ImportExport />,
            action: () =>
                handleAsyncTask({
                    task: () => importAnnualFinancialStatementTransactions(annualFinancialStatementId!),
                    onLoading: setLoading,
                    onSuccess: loadData,
                    onError: setError
                })
        }
    ];

    useEffect(() => {
        loadData();
    }, [annualFinancialStatementId]);

    function loadData() {
        handleAsyncTask({
            task: () => getAnnualFinancialStatement(annualFinancialStatementId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        handleAsyncTask({
            task: () => updateAnnualFinancialStatement(annualFinancialStatementId!, {
                annualFinancialStatementId: data?.id,
                incomes: data?.incomes,
                expenditures: data?.expenditures
            }),
            onLoading: setLoading,
            onSuccess: () => {
                loadData();
                toast("Jahresabschluss erfolgreich gespeichert", "success");
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit} >
            <Stack spacing={2}>
                {renderHeaderBlock(data, loading, error, toolbarItems)}

                <DefaultContentContainer title={"Einnahmen"} loading={loading} toolbarItems={toolbarItemsIncome} error={error} >
                    {data && renderTransactionsBlock(data.incomes!, handleChange, 'incomes', data.totalIncome!, data.currency!, data.period!)}
                </DefaultContentContainer>

                <DefaultContentContainer title={"Ausgaben"} loading={loading} toolbarItems={toolbarItemsIncome} error={error}>
                    {data && renderTransactionsBlock(data.expenditures!, handleChange, 'expenditures', data.totalExpenditure!, data.currency!, data.period!)}
                </DefaultContentContainer>

                <DetailsActions formId="form" deleteUrl={`/financials/annualstatements/${data?.id?.value}/delete`} disabled={loading} />
            </Stack>
        </form>
    );
};

export default AnnualFinancialStatementDetails;

const renderHeaderBlock = (data: AnnualFinancialStatementDTO | undefined, loading: boolean, error: AppError | undefined, toolbarItems: ToolbarItem[]) => {
    return <DefaultContentContainer subtitle={data?.displayName} loading={loading} error={error} toolbarItems={toolbarItems}>
        <Paper sx={{ p: 2 }}>
            <Grid container spacing={2}>
                <Grid size={{ xs: 6 }}>
                    <TextField
                        type="date"
                        fullWidth
                        id="period.from"
                        name="period.from"
                        label="Datum von"
                        value={data?.period?.from}
                        disabled />
                </Grid>
                <Grid size={{ xs: 6 }}>
                    <TextField
                        type="date"
                        fullWidth
                        id="period.to"
                        name="period.to"
                        label="Datum bis"
                        value={data?.period?.to}
                        disabled />
                </Grid>
                <Grid size={{ xs: 6, sm: 4 }}>
                    <TextField
                        type='number'
                        fullWidth
                        id="totalIncome"
                        name="totalIncome"
                        label="Einnahmen"
                        value={data?.totalIncome}
                        disabled />
                </Grid>
                <Grid size={{ xs: 6, sm: 4 }}>
                    <TextField
                        type='number'
                        fullWidth
                        id="totalExpenditure"
                        name="totalExpenditure"
                        label="Ausgaben"
                        value={data?.totalExpenditure}
                        disabled />
                </Grid>
                <Grid size={{ xs: 12, sm: 4 }}>
                    <TextField
                        type='number'
                        fullWidth
                        id="netIncome"
                        name="netIncome"
                        label="Gewinn/Verlust"
                        value={data?.netIncome}
                        disabled />
                </Grid>
            </Grid>
        </Paper>
    </DefaultContentContainer>;
}

const renderTransactionsBlock = (transactions: FinancialTransaction[], handleChange: (e: any) => void, transactionsName: 'incomes' | 'expenditures', total: number, currency: Currency, period: DatePeriod) => {
    const add = () => {
        transactions.push({ date: new Date(), description: '', amount: 0, category: { value: '' } });
        handleChange({ target: { name: transactionsName, value: transactions } });
    }

    const remove = (transaction: any) => () => {
        transactions.splice(transactions.indexOf(transaction), 1);
        handleChange({ target: { name: transactionsName, value: transactions } });
    }

    const prepareHandleChange = (e: any, item: FinancialTransaction) => {
        let { name, value } = e.target;
        item[name as keyof FinancialTransaction] = value;
        handleChange({ target: { name: transactionsName, value: transactions } });
    }

    return <TableContainer component={Paper}>
            <Table width="100%">
                <TableHead>
                    <TableRow>
                        <TableCell></TableCell>
                        <TableCell>Datum</TableCell>
                        <TableCell>Beschreibung</TableCell>
                        <TableCell>Kategorie</TableCell>
                        <TableCell align='right'>Betrag</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {transactions.map((transaction, index) => (
                        <TableRow key={index}>
                            <TableCell>
                                {transaction.isImported && <IconButton size="small" title='Importiert'>
                                    <Lock fontSize="small" />
                                </IconButton>}                                
                                {!transaction.isImported && <IconButton size="small" title=''
                                    onClick={remove(transaction)}>
                                    <Remove fontSize="small" />
                                </IconButton>}
                            </TableCell>
                            <TableCell>
                                <TextField
                                    fullWidth
                                    type='date'
                                    size='small'
                                    value={formatForTextField(transaction.date, 'date')}
                                    name={`date`}
                                    onChange={(e) => prepareHandleChange(e, transaction)}
                                    disabled={transaction.isImported}
                                    variant="standard"
                                    slotProps={{
                                        input: { disableUnderline: true },
                                        htmlInput: {
                                            max: new Date().toISOString().split("T")[0],
                                            min: period?.from
                                        }
                                    }}
                                />
                            </TableCell>
                            <TableCell component="th" scope="row">
                                <TextField
                                    fullWidth
                                    size='small'
                                    value={transaction.description}
                                    name={`description`}
                                    onChange={(e) => prepareHandleChange(e, transaction)}
                                    disabled={transaction.isImported}
                                    variant="standard"
                                    slotProps={{ input: { disableUnderline: true } }}
                                />
                            </TableCell>
                            <TableCell>
                                    <FinancialCategorySelectField 
                                        name='category'
                                        label=""
                                        value={transaction.category?.value}
                                        type={transactionsName} 
                                        required={true} 
                                        disabled={transaction.isImported}
                                        onChange={(e) => prepareHandleChange(e, transaction)}
                                        slotProps={{ input: { disableUnderline: true } }} />
                            </TableCell>
                            <TableCell align='right'>
                                <TextField
                                    type='number'
                                    size='small'
                                    value={transaction.amount}
                                    name={`amount`}
                                    onChange={(e) => prepareHandleChange(e, transaction)}
                                    disabled={transaction.isImported}
                                    variant='standard'
                                    slotProps={{
                                        input: {
                                            disableUnderline: true,
                                            endAdornment: <InputAdornment position="end">{currency?.sign}</InputAdornment>
                                        },
                                        htmlInput: { style: { textAlign: 'right' } }
                                    }}
                                />
                            </TableCell>
                        </TableRow>
                    ))}
                    <TableRow key={"total"}>
                        <TableCell>
                            <IconButton onClick={add} size="small" title="Neu">
                                <AddCircle />
                            </IconButton>
                        </TableCell>
                        <TableCell component="th" scope="row"></TableCell>
                        <TableCell></TableCell>
                        <TableCell></TableCell>
                        <TableCell align='right'><b>{total.toFixed(2)} {currency?.sign}</b></TableCell>
                    </TableRow>
                </TableBody>
            </Table>
    </TableContainer>
}