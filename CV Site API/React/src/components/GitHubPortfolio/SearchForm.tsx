import type React from "react"

import { useState } from "react"
import { Box, TextField, Button, Grid, InputAdornment, Paper, Stack } from "@mui/material"
import SearchIcon from "@mui/icons-material/Search"
import CodeIcon from "@mui/icons-material/Code"
import PersonIcon from "@mui/icons-material/Person"
import ClearIcon from "@mui/icons-material/Clear"
import ViewListIcon from "@mui/icons-material/ViewList"

type Props = {
  onSearch: (params: { name?: string; language?: string; user?: string }) => void
  onViewAll: () => void
}

export default function SearchForm({ onSearch, onViewAll }: Props) {
  const [name, setName] = useState("")
  const [language, setLanguage] = useState("")
  const [user, setUser] = useState("")

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSearch({ name, language, user })
  }

  const handleClear = () => {
    setName("")
    setLanguage("")
    setUser("")
    onSearch({})
  }

  const hasValues = name || language || user

  return (
    <Paper
      elevation={1}
      sx={{
        p: 3,
        borderRadius: 2,
        bgcolor: "rgba(138, 51, 253, 0.03)",
        border: "1px solid rgba(138, 51, 253, 0.1)",
      }}
    >
      <Box component="form" onSubmit={handleSubmit} noValidate>
        <Grid container spacing={2}>
          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              placeholder="Repository name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              variant="outlined"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon sx={{ color: "rgba(138, 51, 253, 0.6)" }} />
                  </InputAdornment>
                ),
              }}
              sx={{
                "& .MuiOutlinedInput-root": {
                  "&:hover fieldset": {
                    borderColor: "rgba(138, 51, 253, 0.3)",
                  },
                  "&.Mui-focused fieldset": {
                    borderColor: "#8a33fd",
                  },
                },
              }}
            />
          </Grid>

          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              placeholder="Language"
              value={language}
              onChange={(e) => setLanguage(e.target.value)}
              variant="outlined"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <CodeIcon sx={{ color: "rgba(138, 51, 253, 0.6)" }} />
                  </InputAdornment>
                ),
              }}
              sx={{
                "& .MuiOutlinedInput-root": {
                  "&:hover fieldset": {
                    borderColor: "rgba(138, 51, 253, 0.3)",
                  },
                  "&.Mui-focused fieldset": {
                    borderColor: "#8a33fd",
                  },
                },
              }}
            />
          </Grid>

          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              placeholder="Username"
              value={user}
              onChange={(e) => setUser(e.target.value)}
              variant="outlined"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <PersonIcon sx={{ color: "rgba(138, 51, 253, 0.6)" }} />
                  </InputAdornment>
                ),
              }}
              sx={{
                "& .MuiOutlinedInput-root": {
                  "&:hover fieldset": {
                    borderColor: "rgba(138, 51, 253, 0.3)",
                  },
                  "&.Mui-focused fieldset": {
                    borderColor: "#8a33fd",
                  },
                },
              }}
            />
          </Grid>
        </Grid>

        <Stack direction="row" spacing={1} justifyContent="flex-end" mt={2}>
          {/* View All button */}
          <Button
            type="button"
            variant="outlined"
            startIcon={<ViewListIcon />}
            onClick={onViewAll}
            sx={{
              borderColor: "rgba(138, 51, 253, 0.5)",
              color: "#8a33fd",
              "&:hover": {
                borderColor: "#8a33fd",
                backgroundColor: "rgba(138, 51, 253, 0.04)",
              },
            }}
          >
            View All
          </Button>

          {/* Clear button - only shown when there are values */}
          {hasValues && (
            <Button
              variant="outlined"
              onClick={handleClear}
              startIcon={<ClearIcon />}
              sx={{
                borderColor: "rgba(138, 51, 253, 0.5)",
                color: "#8a33fd",
                "&:hover": {
                  borderColor: "#8a33fd",
                  backgroundColor: "rgba(138, 51, 253, 0.04)",
                },
              }}
            >
              Clear
            </Button>
          )}

          {/* Search button */}
          <Button
            type="submit"
            variant="contained"
            startIcon={<SearchIcon />}
            sx={{
              bgcolor: "#8a33fd",
              "&:hover": {
                bgcolor: "#7928da",
              },
            }}
          >
            Search
          </Button>
        </Stack>
      </Box>
    </Paper>
  )
}
