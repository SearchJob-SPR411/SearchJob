import { Box, Typography, Button } from "@mui/material";
import { Link } from "react-router-dom";


export default function VacancyCard({ vacancy }) {
    const salary =
        vacancy.salaryMin !== null || vacancy.salaryMax !== null
            ? `${vacancy.salaryMin ?? "—"} – ${vacancy.salaryMax ?? "—"} ₴`
            : "Salary not specified";

    return (
        <Box
            sx={{
                p: 3,
                border: 1,
                borderColor: "divider",
                borderRadius: 2,
                backgroundColor: "background.paper",
            }}
        >
            <Typography variant="h5">
                {vacancy.title}
            </Typography>

            <Typography
                color="primary"
                sx={{ mt: 0.5 }}
            >
                {vacancy.companyName}
            </Typography>

            <Typography
                color="text.secondary"
                sx={{ mt: 1 }}
            >
                {vacancy.location} &nbsp; • &nbsp;
                {vacancy.employmentType} &nbsp; • &nbsp;
                {vacancy.workFormat}
            </Typography>

            <Typography sx={{ mt: 2 }}>
                {salary}
            </Typography>

            <Button
                component={Link}
                to={`/vacancies/${vacancy.id}`}
                variant="contained"
                sx={{ mt: 2, alignSelf: "flex-start" }}
            >
                View vacancy
            </Button>

        </Box>
    );
}