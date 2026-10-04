import { Box } from "@mui/material";

export default function DefaultLayout({ children }) {
  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "flex",
        flexDirection: "column",
      }}
    >
      {children}
    </Box>
  );
}