using System;
using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class AppointmentProcessor
    {
        private readonly List<Action<Appointment>> _actions =
            new List<Action<Appointment>>();

        public AppointmentProcessor Run(Action<Appointment> action)
        {
            _actions.Add(action);
            return this;
        }

        public AppointmentProcessor RunIf(
            Func<Appointment, bool> predicate,
            Action<Appointment> action)
        {
            return Run(appointment =>
            {
                if (predicate(appointment))
                {
                    action(appointment);
                }
            });
        }

        public AppointmentProcessor Combine(
            Action<Appointment> first,
            Action<Appointment> second)
        {
            return Run(appointment =>
            {
                first(appointment);
                second(appointment);
            });
        }

        public void Execute(IEnumerable<Appointment> source)
        {
            foreach (Appointment appointment in source)
            {
                foreach (Action<Appointment> action in _actions)
                {
                    action(appointment);
                }
            }
        }

        public void Clear()
        {
            _actions.Clear();
        }
    }
}