import { useState, useEffect } from "react";
import { Box, Typography, Button, IconButton, Chip, Tooltip } from "@mui/material";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import BookmarkBorderIcon from "@mui/icons-material/BookmarkBorder";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import { Link } from "react-router-dom";
import { isLocalFavorite, toggleFavorite } from "../../services/favoritesService";
import { hasApplied } from "../../services/applicationService";

export default function VacancyCard({ vacancy }) {
    const [isFav, setIsFav] = useState(false);
    const [appliedInfo, setAppliedInfo] = useState(null);

    useEffect(() => {
        setIsFav(isLocalFavorite(vacancy.id));
        setAppliedInfo(hasApplied(vacancy.id));

        const handleFavUpdate = () => {
            setIsFav(isLocalFavorite(vacancy.id));
        };

        const handleAppUpdate = () => {
            setAppliedInfo(hasApplied(vacancy.id));
        };

        window.addEventListener("favorites-changed", handleFavUpdate);
        window.addEventListener("applications-changed", handleAppUpdate);

        return () => {
            window.removeEventListener("favorites-changed", handleFavUpdate);
            window.removeEventListener("applications-changed", handleAppUpdate);
        };
    }, [vacancy.id]);

    const handleToggleFavorite = async (e) => {
        e.preventDefault();
        e.stopPropagation();
        const newState = await toggleFavorite(vacancy);
        setIsFav(newState);
    };

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
                transition: "border-color 0.2s, box-shadow 0.2s",
                "&:hover": {
                    borderColor: "primary.main",
                    boxShadow: 2,
                },
            }}
        >
            <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", gap: 2 }}>
                <Box sx={{ flexGrow: 1 }}>
                    <Box sx={{ display: "flex", alignItems: "center", gap: 1, flexWrap: "wrap" }}>
                        <Typography variant="h5">
                            {vacancy.title}
                        </Typography>
                        {appliedInfo && (
                            <Chip
                                icon={<CheckCircleIcon />}
                                label={`Подано (${appliedInfo.status || "Pending"})`}
                                color="success"
                                size="small"
                                variant="outlined"
                            />
                        )}
                    </Box>

                    <Typography
                        color="primary"
                        sx={{ mt: 0.5, fontWeight: "medium" }}
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

                    <Typography sx={{ mt: 2, fontWeight: "bold" }}>
                        {salary}
                    </Typography>
                </Box>

                <Tooltip title={isFav ? "Видалити з обраного" : "Додати в обране"}>
                    <IconButton
                        onClick={handleToggleFavorite}
                        color={isFav ? "primary" : "default"}
                        sx={{ p: 1 }}
                        aria-label="favorite"
                    >
                        {isFav ? <BookmarkIcon /> : <BookmarkBorderIcon />}
                    </IconButton>
                </Tooltip>
            </Box>

            <Box sx={{ display: "flex", gap: 2, mt: 2 }}>
                <Button
                    component={Link}
                    to={`/vacancies/${vacancy.id}`}
                    variant="contained"
                >
                    Переглянути вакансію
                </Button>
            </Box>
        </Box>
    );
}