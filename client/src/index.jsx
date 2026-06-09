import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import AppointmentsList from "./components/appointments/AppointmentsList";
import NewAppointmentForm from "./components/appointments/NewAppointmentForm";
import EditAppointmentForm from "./components/appointments/EditAppointmentForm";
import CustomersList from "./components/customers/CustomersList";
import NewCustomerForm from "./components/customers/NewCustomerForm";
import StylistsList from "./components/stylists/StylistsList";
import NewStylistForm from "./components/stylists/NewStylistForm";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <BrowserRouter>
    <Routes>
      <Route path="/" element={<App />}>
        <Route index element={<AppointmentsList />} />
        <Route path="appointments">
          <Route index element={<AppointmentsList />} />
          <Route path="new" element={<NewAppointmentForm />} />
          <Route path=":id/edit" element={<EditAppointmentForm />} />
        </Route>
        <Route path="customers">
          <Route index element={<CustomersList />} />
          <Route path="new" element={<NewCustomerForm />} />
        </Route>
        <Route path="stylists">
          <Route index element={<StylistsList />} />
          <Route path="new" element={<NewStylistForm />} />
        </Route>
      </Route>
    </Routes>
  </BrowserRouter>,
);
