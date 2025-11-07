import { useEffect, useState } from "react";
import { Paper } from "@mui/material";
import DefaultContentContainer, { ToolbarItem } from "../../components/ContentBlocks/DefaultContentBlock";
import { Add, Edit } from "@mui/icons-material";
import { handleAsyncTask } from "../../utils/handleAsyncTask";
import { PaymentDTO } from "../../types/domain";
import { getPayments } from "../../services/payments.services";
import { formatDate } from "../../utils/formatDate";
import { DataGrid, GridActionsCellItem, GridColDef } from "@mui/x-data-grid";
import { Link } from "react-router-dom";

const columns: GridColDef[] = [
    { field: 'number', headerName: '#', width: 120, valueGetter: (value: any) => value.displayName },
    { field: 'direction', headerName: 'Richtung', width: 80, valueGetter: (value: any) => value.value },
    { field: 'status', headerName: 'Status', width: 80, valueGetter: (value: any) => value.value },
    { field: 'payer', headerName: 'Zahlender', flex: 1},
    { field: 'payee', headerName: 'Zahlungsempfänger', flex: 1, },
    { field: 'dueDate', headerName: 'Fällig am', width: 100, valueFormatter: (value) => formatDate(value) },
    {
        field: 'actions',
        type: 'actions',
        headerName: '',
        width: 100,
        getActions: ({ id }) => {
            return [
                <GridActionsCellItem
                    icon={<Edit />}
                    label="Öffnen"
                    className="textPrimary"
                    component={Link}
                    onClick={() => window.location.href = `/payments/${id}`}
                    color="primary"
                />,
            ];
        },
    }
];

const paginationModel = { page: 0, pageSize: 10 };

const PaymentList = () => {
    const [data, setData] = useState<PaymentDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [
        {
            text: "Neu",
            route: "/payments/create",
            icon: <Add />
        }
    ];

    useEffect(() => {
        handleAsyncTask({
            task: () => getPayments(),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    return (
        <DefaultContentContainer loading={loading} error={error} toolbarItems={toolbarItems}>
            <Paper>
                <DataGrid
                        rows={data.sort((a, b) => b.number?.value! - a.number?.value!)}
                        columns={columns}
                        getRowId={(row) => row.id.value}
                        isRowSelectable={() => false}
                        initialState={{ pagination: { paginationModel } }}
                        pageSizeOptions={[10, 25, 50, 100]}
                        sx={{ border: 0 }}
                    />
            </Paper>
        </DefaultContentContainer>
    );
}

export default PaymentList;