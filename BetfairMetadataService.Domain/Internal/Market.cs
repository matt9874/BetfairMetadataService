﻿using System;
using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class Market : BaseEntity
    {
        public string MarketName { get; set; }
        public bool? MarketDataDelayed { get; set; }
        public ICollection<Selection> Runners { get; set; }
        public Event Event { get; set; }
        public string EventId { get; set; }
        public bool? PersistenceEnabled { get; set; }
        public bool? BspMarket { get; set; }
        public DateTime MarketTime { get; set; }
        public DateTime? SuspendTime { get; set; }
        public DateTime? SettleTime { get; set; }
        public MarketBettingType BettingType { get; set; }
        public bool? TurnInPlayEnabled { get; set; }
        public string MarketType { get; set; }
        public string Regulator { get; set; }
        public double? MarketBaseRate { get; set; }
        public bool? DiscountAllowed { get; set; }
        public string Wallet { get; set; }
        public string Rules { get; set; }
        public bool? RulesHasDate { get; set; }
        public string Clarifications { get; set; }
        public double? TotalMatched { get; set; }
    }
}
