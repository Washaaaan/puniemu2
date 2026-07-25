import os

OUTPUT_DIR = "field_defs"

fields = {}

def set_field(name, value):
    fields[name] = value

# --- list<object> fields ---
list_fields = [
    "ywp_user_youkai_collect",
    "ywp_user_youkai_intro",
    "ywp_user_goku_youkai_intro_release",
    "ywp_user_goku_story",
    "ywp_user_friend_request_recv",
    "ywp_user_friend",
    "ywp_user_present_box_list",
    "ywp_user_crystal_menu",
    "ywp_user_drive_progress",
    "ywp_user_event_point",
    "ywp_user_event_point_trade",
    "ywp_user_event_ranking_reward",
    "ywp_user_event_tutorial",
    "ywp_user_friend_stage",
    "ywp_user_gacha",
    "ywp_user_medal_point_trade",
    "ywp_user_mini_game_map",
    "ywp_user_mini_game_map_friend",
    "ywp_user_raid_boss",
    "ywp_user_score_attack_reward",
    "ywp_user_stage_rank",
    "ywp_user_stage_relation_progress",
    "ywp_user_steal_progress",
]

for f in list_fields:
    set_field(f, "[]")

# --- null string fields ---
null_fields = [
    "ywp_user_league_rank",
    "ywp_user_gacha_stamp",
    "ywp_user_youkai_strong_skill",
    "ywp_user_youkai_legend_release_history",
    "ywp_user_youkai_bonus_effect",
    "ywp_user_treasure_series",
    "ywp_user_treasure",
    "ywp_user_shop_item_unlock",
    "ywp_user_item",
    "ywp_user_event_progress",
    "ywp_user_conflate",
]

for f in null_fields:
    set_field(f, "\"\"")

# --- overrides exactly like your C# calls ---
overrides = [
    "ywp_user_youkai_collect",
    "ywp_user_youkai_intro",
    "ywp_user_goku_youkai_intro_release",
    "ywp_user_goku_story",
    "ywp_user_crystal_menu",
    "ywp_user_event_point",
    "ywp_user_mini_game_map",
    "ywp_user_mini_game_map_friend",
    "ywp_user_stage_relation_progress",
]

for f in overrides:
    set_field(f, "[]")

set_field("ywp_user_league_rank", "")
set_field("ywp_user_gacha_stamp", "")
set_field("ywp_user_youkai_strong_skill", "")
set_field("ywp_user_youkai_legend_release_history", "")
set_field("ywp_user_youkai_bonus_effect", "")

# --- write files ---
os.makedirs(OUTPUT_DIR, exist_ok=True)

for name, value in fields.items():
    with open(os.path.join(OUTPUT_DIR, f"{name}_def.txt"), "w", encoding="utf-8") as f:
        f.write(value)

print(f"Generated {len(fields)} files in {OUTPUT_DIR}")