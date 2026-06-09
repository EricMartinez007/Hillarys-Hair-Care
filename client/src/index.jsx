import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import AppointmentsList from "./components/appointments/AppointmentsList";
import NewAppointmentForm from "./components/appointments/NewAppointmentForm";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <BrowserRouter>
    <Routes>
      <Route path="/" element={<App />}>
        <Route index element={<AppointmentsList />} />
        <Route path="appointments">
          <Route index element={<AppointmentsList />} />
          <Route path="new" element={<NewAppointmentForm />} />
        </Route>
      </Route>
    </Routes>
  </BrowserRouter>,
);
