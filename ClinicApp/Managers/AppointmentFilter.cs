using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class AppointmentFilter
    {
        private Func<Appointment, bool>? _predicate;

        public AppointmentFilter Add(Func<Appointment, bool> predicate)
        {
            var previous = _predicate;

            if (previous == null)
            {
                _predicate = predicate;
            }
            else
            {
                _predicate = a => previous(a) && predicate(a);
            }

            return this;
        }

        public AppointmentFilter And(Func<Appointment, bool> predicate)
        {
            return Add(predicate);
        }

        public AppointmentFilter Or(Func<Appointment, bool> predicate)
        {
            var previous = _predicate;

            if (previous == null)
            {
                _predicate = predicate;
            }
            else
            {
                _predicate = a => previous(a) || predicate(a);
            }

            return this;
        }

        public AppointmentFilter Negate()
        {
            var previous = _predicate;

            if (previous != null)
            {
                _predicate = a => !previous(a);
            }

            return this;
        }

        public IEnumerable<Appointment> Apply(IEnumerable<Appointment> source)
        {
            var predicate = _predicate;
            return predicate == null ? source : source.Where(predicate);
        }

        public void Reset()
        {
            _predicate = null;
        }
    }
}